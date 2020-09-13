using Contract.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AnimalDistributorService.Api.Services
{
    public class LocalStorageService : IStorageService
    {
        private const string MediaRootFoler = "user-content";
        private const string WebRootPath = "Media";

        public string GetMediaUrl(Guid animalIdentifier, MediaType mediaType, string fileDirectory, string fileName, int animalType)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                animalIdentifier = Guid.Empty;
                mediaType = MediaType.Image;
                fileName = animalType == 1 ? DummyImagesStorage.Doggy : DummyImagesStorage.Catty;
            }
            return $"/{MediaRootFoler}/{animalIdentifier.ToString()}/{((int)mediaType).ToString()}/{fileDirectory}/{fileName}";
        }

        public async Task SaveMediaAsync(Stream mediaBinaryStream, string dirs, string fileName)
        {
            var filePath = Path.Combine(WebRootPath, MediaRootFoler, dirs, fileName);

            if (Directory.CreateDirectory(Path.Combine(WebRootPath, MediaRootFoler, dirs)).Exists)
            {
                using (var output = new FileStream(filePath, FileMode.Create))
                {
                    await mediaBinaryStream.CopyToAsync(output);
                }
            }
            else
            {
                throw new Exception("Directory issue");
            }
        }

        public async Task DeleteMediaAsync(Guid animalIdentifier, MediaType mediaType, string fileName)
        {
            var filePath = Path.Combine(WebRootPath, MediaRootFoler, fileName);
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
}
