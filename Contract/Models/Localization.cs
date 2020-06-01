using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace Contract.Models
{
    public class Localization
    {
        public virtual Guid LocalizationId { get; protected set; }
        public virtual string Country { get; protected set; }
        public virtual string City { get; protected set; }
        public virtual string Street { get; protected set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Animal Animal { get; protected set; }
        public virtual Guid AnimalRef { get; protected set; }

        public Localization(string country, string city, string street)
        {
            Country = country;
            City = city;
            Street = street;
        }
        public static Localization Of(string country, string city, string street)
        {
            return new Localization(country, city, street);
        }
    }
}
