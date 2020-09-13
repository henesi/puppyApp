using System.Threading;
using System.Threading.Tasks;
using AnimalDistributorService.Api.Commands;
using AnimalDistributorService.DataAccess.EntityFramework;
using AnimalDistributorService.MessageBroker.Producers;
using Contract.Models;
using MediatR;
using Newtonsoft.Json;

namespace AnimalDistributorService.Commands
{
    public class CreateAnimalHandler : IRequestHandler<CreateAnimalCommand, CreateAnimalResult>
    {
        private readonly IRepository<Animal> _animalRepository;
        private readonly AnimalCreationProducer _publisher;
        public CreateAnimalHandler(IRepository<Animal> animalRepository, AnimalCreationProducer publisher)
        {
            this._animalRepository = animalRepository;
            this._publisher = publisher;
        }

        public async Task<CreateAnimalResult> Handle(CreateAnimalCommand request, CancellationToken cancellationToken)
        {
            var o = new Animal(
                request.Alias,
                request.Description,
                Localization.Of(
                    request.Localization.Country,
                    request.Localization.City,
                    request.Localization.Street),
                request.AnimalType,
                request.CreatorId
                );

            //add animal to DB
            await _animalRepository.Add(o);

            //update elastic search
            await _publisher.Send(o);

            return new CreateAnimalResult
            {
                AnimalId = o.AnimalId
            };
        }
    }
}
