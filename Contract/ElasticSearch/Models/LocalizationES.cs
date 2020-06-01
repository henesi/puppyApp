using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Contract.Models
{
    public class LocalizationES
    {
        public string LocalizationId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}
