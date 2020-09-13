﻿using AnimalDistributorService.Api.Commands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.Api.Commands.Dtos
{
    public class CreateAnimalCommandDto : IRequest<CreateAnimalResult>
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public LocalizationDto Localization { get; set; }
        public int AnimalType { get; set; }
    }
}
