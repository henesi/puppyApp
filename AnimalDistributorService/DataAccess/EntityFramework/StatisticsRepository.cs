using Contract.Models;
using Contract.Models.ComputerVision;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public class StatisticsRepository : IRepository<Statistics>
    {
        private readonly AnimalDBContext _animalDBContext;

        public StatisticsRepository(AnimalDBContext animalDBContext)
        {
            this._animalDBContext = animalDBContext;
        }

        public async Task<Statistics> Add(Statistics var)
        {
            _animalDBContext.Entry(var).State = (var.StatisticId == null ? EntityState.Modified : EntityState.Added);
            await _animalDBContext.SaveChangesAsync();
            return var;
        }

        public Statistics Get(Guid var)
        {
            return _animalDBContext
               .CV_Statistics.Where(x => x.StatisticId == var).FirstOrDefault();
        }

        public async void Remove(Statistics var)
        {
            _animalDBContext.Remove(var);
            await _animalDBContext.SaveChangesAsync();
        }

        public IQueryable<Statistics> GetAll(Guid var)
        {
            return _animalDBContext
               .CV_Statistics.Where(x => x.StatisticId == var);
        }
    }
}
