using Contract.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnimalDistributorService.Api.Core
{
    public class FileHelper
    {
        private string[] imageTypes = new[] { "image/jpeg", "image/png" };
        private string[] videoTypes = new[] { "video/mp4", "video/avi" };
        private string[] fileTypes = new[] { "not_supported" };

        public int GetFileType(IFormFile file)
        {
            if (this.imageTypes.Contains(file.ContentType))
            {
                return (int)MediaType.Image;
            }
            else if (this.videoTypes.Contains(file.ContentType))
            {
                return (int)MediaType.Video;
            }
            else throw new Exception("Not supported");
        }
        public string GenerateFileName(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Value.Trim('"');
            return $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        }
    }
}
