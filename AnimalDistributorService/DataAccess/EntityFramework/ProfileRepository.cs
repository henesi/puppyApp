using AnimalDistributorService.Api.Services;
using Contract.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public class ProfileRepository : IRepository<Profile>
    {
        private readonly AnimalDBContext _animalDBContext;

        public ProfileRepository(AnimalDBContext animalDBContext)
        {
            this._animalDBContext = animalDBContext;
        }

        public async Task<Profile> Add(Profile var)
        {
            _animalDBContext.Entry(var).State = (var.ProfileId == null ? EntityState.Modified : EntityState.Added);
            await _animalDBContext.SaveChangesAsync();
            return var;
        }

        public Profile Get(Guid var)
        {
            return _animalDBContext
               .Profile.Where(x => x.ProfileId == var).FirstOrDefault();
        }

        public async void Remove(Profile var)
        {
            _animalDBContext.Remove(var);
            await _animalDBContext.SaveChangesAsync();
        }

        public IQueryable<Profile> GetAll(Guid var)
        {
            return _animalDBContext
               .Profile.Where(x => x.AnimalRef == var);
        }
    }
}
