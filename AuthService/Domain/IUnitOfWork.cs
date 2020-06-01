using System;
using System.Threading.Tasks;

namespace AuthService.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        Task CommitChanges();
    }
}
