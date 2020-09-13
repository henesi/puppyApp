using Contract.Models;
using Contract.Models.ComputerVision;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public class RejectionRepository : IRepository<Rejection>
    {
        private readonly AnimalDBContext _animalDBContext;

        public RejectionRepository(AnimalDBContext animalDBContext)
        {
            this._animalDBContext = animalDBContext;
        }

        public async Task<Rejection> Add(Rejection var)
        {
            _animalDBContext.Entry(var).State = (var.RejectionId == null ? EntityState.Modified : EntityState.Added);
            await _animalDBContext.SaveChangesAsync();
            return var;
        }

        public Rejection Get(Guid var)
        {
            return _animalDBContext
               .CV_Rejection.Where(x => x.RejectionId == var).FirstOrDefault();
        }

        public async void Remove(Rejection var)
        {
            _animalDBContext.Remove(var);
            await _animalDBContext.SaveChangesAsync();
        }

        public IQueryable<Rejection> GetAll(Guid var)
        {
            return _animalDBContext
               .CV_Rejection.Where(x => x.AnimalRef == var);
        }
    }
}
