using AnimalDistributorService.Api.Services;
using Contract.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AnimalDistributorService.DataAccess.EntityFramework;
using Contract.Minio;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Contract.Models.ComputerVision;
using AnimalDistributorService.MessageBroker.Producers;

namespace AnimalDistributorService.MessageBroker.Consumers
{
    public class AnimalProfileComputedConsumertedConsumer : IConsumer<Profile>
    {
        private readonly ILogger<AnimalProfileComputedConsumertedConsumer> _logger;
        private readonly IRepository<Profile> _profileRepository;
        private readonly IRepository<Animal> _animalRepository;
        private readonly IStorageService _storageService;
        private readonly AnimalCreationProducer _animalCreationProducer;
        private readonly IRepository<Rejection> _rejectionRepository;

        public AnimalProfileComputedConsumertedConsumer(ILogger<AnimalProfileComputedConsumertedConsumer> logger, IRepository<Profile> profileRepository, 
            IRepository<Animal> animalRepository, IRepository<Rejection> rejectionRepository, IStorageService storageService, AnimalCreationProducer animalCreationProducer)
        {
            this._logger = logger;
            this._profileRepository = profileRepository;
            this._animalRepository = animalRepository;
            this._storageService = storageService;
            this._animalCreationProducer = animalCreationProducer;
            this._rejectionRepository = rejectionRepository;
        }
        public async Task Consume(ConsumeContext<Profile> context)
        {
            _logger.LogDebug("Animal profile computed:" + context.Message.FileName);

            List<DetectionModel> detectionResult;

            //get animal
            var animal = this._animalRepository.Get(context.Message.AnimalRef);

            //get mapping file
            var mappingUrl = _storageService.GetMediaUrl(context.Message.AnimalRef, context.Message.MediaType, context.Message.FileName, FileNames.MAPPING_FILE);

            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(mappingUrl);
                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var sr = new StreamReader(mem))
                    {
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {
                            detectionResult = new JsonSerializer().Deserialize<List<DetectionModel>>(jsonTextReader);
                        }
                    }
                }
            }

            //make a check
            var checkResult = false;
            foreach(var item in detectionResult)
            {
                var animalType = animal.AnimalType.Name.ToLower();
                if (item.name.Equals(animalType))
                {
                    if(item.percentage_probability > 60.0)
                    {
                        var oldProfile = animal.Profile;

                        //mark boolean to true as check is passed
                        checkResult = true;

                        //add profile to db
                        await _profileRepository.Add(new Profile(context.Message.FileName, context.Message.AnimalRef, context.Message.FileSize, (MediaType)context.Message.MediaType));

                        //update profile in elastic db
                        var animalToUpdate = _animalRepository.Get(context.Message.AnimalRef);
                        await _animalCreationProducer.Send(animalToUpdate);

                        //remove old profile in minio
                        if(oldProfile != null) 
                            await _storageService.DeleteMediaAsync(context.Message.AnimalRef, context.Message.MediaType, oldProfile.FileName);

                        break;
                    }
                }
            }

            if (!checkResult)
            {
                //some notification about failure
                await _rejectionRepository.Add(new Rejection(context.Message.FileName, context.Message.AnimalRef, context.Message.MediaType));
            }
        }
    }
}
