using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Contract.Models
{
    public class AnimalType
    {
        public virtual int AnimalTypeId { get; protected set; }
        public virtual string Name { get; protected set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public virtual List<Animal> Animals { get; protected set; }
        protected AnimalType() { } //NH required

        public AnimalType(string name)
        {
            Name = name;
        }
        public AnimalType(int id, string name)
        {
            AnimalTypeId = id;
            Name = name;
        }
        public static AnimalType Of(int id, string name)
        {
            return new AnimalType(id, name);
        }
    }
}
