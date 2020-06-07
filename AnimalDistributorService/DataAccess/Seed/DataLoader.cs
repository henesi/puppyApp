using AnimalDistributorService.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.Seed
{
    public class DataLoader
    {
        private readonly AnimalDBContext dbContext;

        public DataLoader(AnimalDBContext context)
        {
            dbContext = context;
        }

        public void Seed()
        {
            dbContext.Database.Migrate();
            if (dbContext.AnimalType.Any())
            {
                return;
            }

            dbContext.AnimalType.Add(DemoAnimalFactory.Doggy());
            dbContext.AnimalType.Add(DemoAnimalFactory.Kitty());

            dbContext.SaveChanges();
        }
    }
}
