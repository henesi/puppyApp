using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDistributorService.Api.Queries
{
    public class GetAnimalQuery : IRequest<GetAnimalQueryResult>
    {
        public Guid Id { get; set; }
    }
}
