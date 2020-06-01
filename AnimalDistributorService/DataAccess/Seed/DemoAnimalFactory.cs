using Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.Seed
{
    public class DemoAnimalFactory
    {
        internal static AnimalType Doggy()
        {
            var p = new AnimalType("Dog");
            return p;
        }
        internal static AnimalType Kitty()
        {
            var p = new AnimalType("Cat");
            return p;
        }
    }
}
