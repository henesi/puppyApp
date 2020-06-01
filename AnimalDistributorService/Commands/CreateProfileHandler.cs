using System;
using System.Threading;
using System.Threading.Tasks;
using AnimalDistributorService.Api.Commands;
using AnimalDistributorService.DataAccess.EntityFramework;
using AnimalDistributorService.MessageBroker.Producers;
using Contract.Models;
using MediatR;

namespace AnimalDistributorService.Commands
{
    public class CreateProfileHandler : IRequestHandler<AddAnimalProfileCommand, AddAnimalProfileResult>
    {
        private readonly AnimalProfileProducer _animalProfileProducer;

        public CreateProfileHandler(AnimalProfileProducer animalProfileProducer)
        {
            this._animalProfileProducer = animalProfileProducer;
        }
        public async Task<AddAnimalProfileResult> Handle(AddAnimalProfileCommand request, CancellationToken cancellationToken)
        {
            var profile = new Profile(request.FileName, request.AnimalRef, request.FileSize, (MediaType)request.MediaType);

            //send profile to cv service
            await _animalProfileProducer.Send(profile);

            return new AddAnimalProfileResult
            {
                ResultId = request.FileName
            };
        }
    }
}
