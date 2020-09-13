using AnimalDistributorService.Api.Commands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.Api.Commands
{
    public class CreateAnimalCommand : IRequest<CreateAnimalResult>
    {
        public string Alias { get; set; }
        public string Description { get; set; }

        public LocalizationDto Localization { get; set; }
        public int AnimalType { get; set; }
        public Guid CreatorId { get; set; }
    }
}
