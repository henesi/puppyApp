using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDistributorService.Api.Commands.Dtos
{
    public class LocalizationDto
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
