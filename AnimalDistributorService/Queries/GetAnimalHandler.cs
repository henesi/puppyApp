using AnimalDistributorService.Api.Commands.Dtos;
using AnimalDistributorService.Api.Queries;
using AnimalDistributorService.Api.Services;
using AnimalDistributorService.DataAccess.EntityFramework;
using Contract.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Queries
{
    public class GetAnimalHandler : IRequestHandler<GetAnimalQuery, GetAnimalQueryResult>
    {
        private readonly IRepository<Animal> _animalRepository;
        private readonly IStorageService _storageService;

        public GetAnimalHandler(IRepository<Animal> animalRepository, IStorageService storageService)
        {
            this._animalRepository = animalRepository;
            this._storageService = storageService;
        }

        public Task<GetAnimalQueryResult> Handle(GetAnimalQuery request, CancellationToken cancellationToken)
        {
            var animal = _animalRepository.Get(request.Id);

            if (animal == null) { throw new Exception("Animal does not exist"); }

            return Task.FromResult(ConstructResult(animal));
        }

        private GetAnimalQueryResult ConstructResult(Animal animal)
        {
            return new GetAnimalQueryResult
            {
                Alias = animal.Alias,
                AnimalType = new AnimalTypeDto
                {
                    id = animal.AnimalType.AnimalTypeId,
                    name = animal.AnimalType.Name
                },
                Localization = new LocalizationDto
                {
                    City = animal.Localization?.City,
                    Country = animal.Localization?.Country,
                    Street = animal.Localization?.Street
                },
                CreatorId = animal.CreatorId,
                Profile = animal.Profile == null ? _storageService.GetMediaUrl(animal.AnimalId.GetValueOrDefault(), MediaType.Image, null, null, animal.AnimalType.AnimalTypeId)
                    : _storageService.GetMediaUrl(animal.AnimalId.GetValueOrDefault(), animal.Profile.MediaType, animal.Profile.FileName, animal.Profile.FileName, animal.AnimalType.AnimalTypeId)
            };
        }
    }
}
