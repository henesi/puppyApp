using AnimalDistributorService.Api.Services;
using AnimalSearchService.DataAccess;
using Contract.ElasticSearch.Models;
using Contract.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AnimalSearchService.MessageBroker.Consumers
{
    public class MyConsumer : IConsumer<Animal>
    {
        private readonly ILogger<Animal> _logger;
        private readonly IAnimalRepository _animalrepository;
        private readonly IStorageService _storageService;

        public MyConsumer(ILogger<Animal> logger, IAnimalRepository repository, IStorageService storageService)
        {
            this._logger = logger;
            this._animalrepository = repository;
            this._storageService = storageService;
        }
        public Task Consume(ConsumeContext<Animal> context)
        {
            _logger.LogDebug("Animal created:" + context.Message.Alias);

            return _animalrepository.Add(new AnimalES
            {
                Alias = context.Message.Alias,
                AnimalId = context.Message.AnimalId.ToString(),
                Profile = context.Message.Profile == null ?
                   _storageService.GetMediaUrl(context.Message.AnimalId.Value, MediaType.Image, null, null, context.Message.AnimalTypeRef)
                    : _storageService.GetMediaUrl(context.Message.AnimalId.Value, context.Message.Profile.MediaType, context.Message.Profile?.FileName, context.Message.Profile?.FileName, context.Message.AnimalTypeRef),
                Localization = new LocalizationES()
                {
                    City = context.Message.Localization.City,
                    Country = context.Message.Localization.Country,
                    LocalizationId = context.Message.Localization.LocalizationId.ToString(),
                    Street = context.Message.Localization.Street
                },
                AnimalTypeRef = context.Message.AnimalTypeRef,
                CreatorId = context.Message.CreatorId.ToString()
            });
        }
    }
}
