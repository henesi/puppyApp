using AnimalDistributorService.Api.Services;
using Contract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public class AnimalRepository : IRepository<Animal>
    {
        private readonly AnimalDBContext _animalDBContext;

        public AnimalRepository(AnimalDBContext animalDBContext)
        {
            this._animalDBContext = animalDBContext;
        }

        public async Task<Animal> Add(Animal var)
        {
            _animalDBContext.Add(var);
            await _animalDBContext.SaveChangesAsync();
            return var;
        }

        public Animal Get(Guid id)
        {
            return _animalDBContext
               .Animals.Where(x => x.AnimalId == id)
               .Include(p => p.Localization)
               .Include(p => p.AnimalType)
               .Include(p => p.Profile)
               .FirstOrDefault();
        }

        public async void Remove(Animal var)
        {
            _animalDBContext.Remove(var);
            await _animalDBContext.SaveChangesAsync();
        }

        public IQueryable<Animal> GetAll(Guid var)
        {
            throw new NotImplementedException();
        }
    }
}
