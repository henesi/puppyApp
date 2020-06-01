using Contract.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDistributorService.Api.Commands
{
    public class AddAnimalProfileCommand : IRequest<AddAnimalProfileResult>
    {
        public int FileSize { get; set; }
        public string FileName { get; set; }
        public int MediaType { get; set; }
        public bool IsProfile { get; set; }
        public virtual Guid AnimalRef { get; set; }
    }
}
