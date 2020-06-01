using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.Models.Dto
{
    public class MediaDto
    {
        public IFormFile ThumbnailImage { get; set; }

        public virtual string AnimalRef { get; set; }
    }
}
