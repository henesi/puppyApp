using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalDistributorService.Api.Queries
{
    public class GetMediaQuery : IRequest<GetMediaQueryResult>
    {
        public Guid UserId { get; set; }
    }
}
