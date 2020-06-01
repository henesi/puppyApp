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

namespace AnimalSearchService.MessageBroker.Consumers
{
    public class AnimalMediaComputedConsumer : IConsumer<Media>
    {
        private readonly ILogger<AnimalMediaComputedConsumer> _logger;
        private readonly IRepository<Media> _mediaRepository;
        private readonly IRepository<Animal> _animalRepository;
        private readonly IStorageService _storageService;

        public AnimalMediaComputedConsumer(ILogger<AnimalMediaComputedConsumer> logger, IRepository<Media> mediaRepository, IRepository<Animal> animalRepository, IStorageService storageService)
        {
            this._logger = logger;
            this._mediaRepository = mediaRepository;
            this._animalRepository = animalRepository;
            this._storageService = storageService;
        }
        public async Task Consume(ConsumeContext<Media> context)
        {
            _logger.LogDebug("Animal media computed:" + context.Message.FileName);

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
                        checkResult = true;
                        await _mediaRepository.Add(new Media(context.Message.FileName, context.Message.AnimalRef, context.Message.FileSize, (MediaType)context.Message.MediaType));
                        break;
                    }
                }
            }

            if (!checkResult)
            {
                //TODO make a cleanup
            }
        }
    }
}
