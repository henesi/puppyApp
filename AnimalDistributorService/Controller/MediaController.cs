using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnimalDistributorService.Api.Commands;
using AnimalDistributorService.Api.Core;
using AnimalDistributorService.Api.Queries;
using AnimalDistributorService.Api.Services;
using Contract.Models;
using Contract.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AnimalDistributorService.Controller
{
    [Route("api/[controller]")]
    public class MediaController : ControllerBase
    {
        private readonly IMediator bus;
        private readonly IMediaService _mediaService;
        private readonly FileHelper _fileHelper;
        public MediaController(IMediator bus, IMediaService mediaService)
        {
            this._mediaService = mediaService;
            this._fileHelper = new FileHelper();
            this.bus = bus;
        }

        //[Authorize(Policy = "IsSuperUser")]
        [HttpPost("media")]
        public async Task<ActionResult> PostMedia(MediaDto media)
        {
            var mediaCommand = new AddAnimalMediaCommand()
            {
                AnimalRef = Guid.Parse(media.AnimalRef),
                FileName = _fileHelper.GenerateFileName(media.ThumbnailImage),
                FileSize = (int)media.ThumbnailImage.Length,
                MediaType = _fileHelper.GetFileType(media.ThumbnailImage)
            };
            var result = await bus.Send(mediaCommand);
            var fileDir = $"{mediaCommand.AnimalRef.ToString()}/{((int)mediaCommand.MediaType).ToString()}/{mediaCommand.FileName}";
            await _mediaService.SaveMediaAsync(media.ThumbnailImage.OpenReadStream(), fileDir, mediaCommand.FileName);
            return new JsonResult(result);
        }

        //[Authorize(Policy = "IsSuperUser")]
        [HttpPost("profile")]
        public async Task<ActionResult> PostProfile(MediaDto media)
        {
            var mediaCommand = new AddAnimalProfileCommand()
            {
                AnimalRef = Guid.Parse(media.AnimalRef),
                FileName = _fileHelper.GenerateFileName(media.ThumbnailImage),
                FileSize = (int)media.ThumbnailImage.Length,
                MediaType = _fileHelper.GetFileType(media.ThumbnailImage)
            };
            var result = await bus.Send(mediaCommand);
            var fileDir = $"{mediaCommand.AnimalRef.ToString()}/{((int)mediaCommand.MediaType).ToString()}/{mediaCommand.FileName}";
            await _mediaService.SaveMediaAsync(media.ThumbnailImage.OpenReadStream(), fileDir, mediaCommand.FileName);
            return new JsonResult(result);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            var mediaList = await bus.Send(new GetMediaQuery() { UserId = id });
            var results = mediaList.list.Select(x => new GetMediaQueryResultItem()
            {
                mediaId = x.mediaId,
                contentType = x.contentType,
                fileName = _mediaService.GetMediaUrl(id, (MediaType)x.contentType, x.fileName, x.fileName)
            });
            return new JsonResult(results);
        }
    }
}
