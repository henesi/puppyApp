using AnimalDistributorService.Api.Services;
using Contract.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.File
{
    public class MediaService : IMediaService
    {
        private readonly IStorageService _storageService;

        public MediaService(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public string GetMediaUrl(Guid animalIdentifier, MediaType mediaType, string fileDirectory, string fileName, int animalType)
        {
            return _storageService.GetMediaUrl(animalIdentifier, mediaType, fileName, fileDirectory, animalType);
        }

        public Task SaveMediaAsync(Stream mediaBinaryStream, string dirs, string fileName)
        {
            return _storageService.SaveMediaAsync(mediaBinaryStream, dirs, fileName);
        }

        public Task DeleteMediaAsync(Guid animalIdentifier, Media media)
        {
            return DeleteMediaAsync(animalIdentifier, media.FileName);
        }

        public Task DeleteMediaAsync(Guid animalIdentifier, string fileName)
        {
            return _storageService.DeleteMediaAsync(animalIdentifier, fileName);
        }
    }
}
