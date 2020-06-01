using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimalDistributorService.DataAccess.EntityFramework
{
    public interface IRepository<T>
    {
        Task<T> Add(T var);
        void Remove(T var);
        IQueryable<T> GetAll(Guid var);
        T Get(Guid var);
    }
}
