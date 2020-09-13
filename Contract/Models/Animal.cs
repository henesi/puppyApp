using Contract.Models.ComputerVision;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contract.Models
{
    public class Animal
    {
        public virtual Guid? AnimalId { get; protected set; }
        public virtual string Alias { get; protected set; }
        public virtual string Description { get; protected set; }

        public virtual Localization Localization { get; protected set; }
        public virtual AnimalType AnimalType { get; protected set; }
        public virtual List<Media> Media { get; protected set; }
        public virtual Profile Profile { get; protected set; }
        public virtual Rejection Rejection { get; protected set; }
        public virtual Guid CreatorId { get; protected set; }
        public virtual int AnimalTypeRef { get; protected set; }
        protected Animal() { } //NH required

        public Animal(
            string Alias,
            string Description,
            Localization Localization,
            int AnimalType,
            Guid creatorId)
        {
            this.Description = Description;
            this.Alias = Alias;
            this.Localization = Localization;
            this.AnimalTypeRef = AnimalType;
            this.CreatorId = creatorId;
        }
    }
}
