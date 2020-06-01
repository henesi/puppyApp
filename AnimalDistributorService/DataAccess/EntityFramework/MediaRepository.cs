using AnimalDistributorService.Api.Services;
using Contract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public class MediaRepository : IRepository<Media>
    {
        private readonly AnimalDBContext _animalDBContext;

        public MediaRepository(AnimalDBContext animalDBContext)
        {
            this._animalDBContext = animalDBContext;
        }

        public async Task<Media> Add(Media var)
        {
            _animalDBContext.Add(var);
            await _animalDBContext.SaveChangesAsync();
            return var;
        }

        public Media Get(Guid var)
        {
            return _animalDBContext
               .Media.Where(x => x.MediaId == var).FirstOrDefault();
        }

        public async void Remove(Media var)
        {
            _animalDBContext.Remove(var);
            await _animalDBContext.SaveChangesAsync();
        }

        public IQueryable<Media> GetAll(Guid var)
        {
            return _animalDBContext
               .Media.Where(x => x.AnimalRef == var);
        }
    }
}
