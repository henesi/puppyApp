using Contract.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDistributorService.Api.Services
{
    public interface IStorageService
    {
        string GetMediaUrl(Guid animalIdentifier, MediaType mediaType, string fileDirectory, string fileName, int animalType = 0);

        Task SaveMediaAsync(Stream mediaBinaryStream, string dirs, string fileName);

        Task DeleteMediaAsync(Guid animalIdentifier, MediaType mediaType, string fileName);
    }
}
