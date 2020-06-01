using AnimalDistributorService.Api.Commands.Dtos;
using AnimalDistributorService.Api.Queries;
using AnimalDistributorService.DataAccess.EntityFramework;
using Contract.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Queries
{
    public class GetMediaHandler : IRequestHandler<GetMediaQuery, GetMediaQueryResult>
    {
        private readonly IRepository<Media> _mediaRepository;

        public GetMediaHandler(IRepository<Media> mediaRepository)
        {
            this._mediaRepository = mediaRepository;
        }

        public Task<GetMediaQueryResult> Handle(GetMediaQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(ConstructResult(_mediaRepository.GetAll(request.UserId)));

        }

        private GetMediaQueryResult ConstructResult(IQueryable<Media> list)
        {
            return new GetMediaQueryResult()
            {
                list = list.Select(x => new GetMediaQueryResultItem()
                {
                    contentType = (int)x.MediaType,
                    fileName = x.FileName,
                    mediaId = x.MediaId
                }).ToList()
            };
        }
    }
}
