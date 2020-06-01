using System.Threading;
using System.Threading.Tasks;
using AnimalDistributorService.Api.Commands;
using AnimalDistributorService.DataAccess.EntityFramework;
using AnimalDistributorService.MessageBroker.Producers;
using Contract.Models;
using MediatR;

namespace AnimalDistributorService.Commands
{
    public class CreateMediaHandler : IRequestHandler<AddAnimalMediaCommand, AddAnimalMediaResult>
    {
        private readonly AnimalMediaProducer _animalMediaProducer;

        public CreateMediaHandler(AnimalMediaProducer animalMediaProducer)
        {
            this._animalMediaProducer = animalMediaProducer;
        }
        public async Task<AddAnimalMediaResult> Handle(AddAnimalMediaCommand request, CancellationToken cancellationToken)
        {
            var media = new Media(request.FileName, request.AnimalRef, request.FileSize, (MediaType)request.MediaType);

            //send media to cv service
            await _animalMediaProducer.Send(media);

            return new AddAnimalMediaResult
            {
                ResultId = request.FileName
            };
        }
    }
}
