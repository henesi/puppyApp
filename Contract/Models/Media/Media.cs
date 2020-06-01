using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Contract.Models
{
    public class Media
    {
        public Guid MediaId { get; set; }
        [StringLength(450)]
        public string Caption { get; set; }
        public int FileSize { get; set; }
        [StringLength(450)]
        public string FileName { get; set; }
        public MediaType MediaType { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Animal Animal { get; set; }
        public virtual Guid AnimalRef { get; set; }
        protected Media() { } //NH required
        public Media(string fileName, Guid animalRef, int fileSize, MediaType mediaType)
        {
            FileName = fileName;
            FileSize = fileSize;
            AnimalRef = animalRef;
            MediaType = mediaType;
        }
    }
}
