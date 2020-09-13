using AnimalDistributorService.Api.Commands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDistributorService.Api.Queries
{
    public class GetAnimalQueryResult
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public Guid CreatorId { get; set; }
        public LocalizationDto Localization { get; set; }
        public AnimalTypeDto AnimalType { get; set; }
        public string Profile { get; set; }
    }

}
