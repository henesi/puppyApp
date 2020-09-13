using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Contract.Models.ComputerVision
{
    public class Rejection
    {
        public Guid RejectionId { get; set; }
        public string FileName { get; set; }
        public MediaType MediaType { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Animal Animal { get; set; }
        public virtual Guid AnimalRef { get; set; }
        public bool Verified { get; set; }
        protected Rejection() { } //NH required
        public Rejection(string fileName, Guid animalRef, MediaType mediaType)
        {
            FileName = fileName;
            AnimalRef = animalRef;
            MediaType = mediaType;
        }
    }
}
