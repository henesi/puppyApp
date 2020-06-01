using System;
using System.Threading.Tasks;
using AuthService.Domain;
using NHibernate;

namespace AuthService.DataAccess.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISession session;
        private readonly ITransaction tx;
        private readonly UserRepository userRepository;

        public IUserRepository User => userRepository;

        public UnitOfWork(ISession session)
        {
            this.session = session;
            tx = session.BeginTransaction();
            userRepository = new UserRepository(session);
        }

        public async Task CommitChanges()
        {
            await tx.CommitAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                tx?.Dispose();
            }

        }
    }
}
