using Contract.Models;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.ElasticSearch.Models
{
    [ElasticsearchType(IdProperty = nameof(AnimalId))]
    public class AnimalES
    {
        public string AnimalId { get; set; }
        public string Alias { get; set; }
        public LocalizationES Localization { get; set; }
        public AnimalTypeES AnimalType { get; set; }
        public string Profile { get; set; }
        [JsonIgnore]
        public string CreatorId { get; set; }
        public int AnimalTypeRef { get; set; }
    }
}
