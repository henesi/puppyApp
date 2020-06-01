using AuthService.Domain;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.DataAccess.NHibernate
{
    public class UserRepository : IUserRepository
    {
        private readonly ISession session;

        public UserRepository(ISession session)
        {
            this.session = session;
        }

        public void Add(User user)
        {
            session.Save(user);
        }

        public async Task<User> WithUsername(string username)
        {
            return await session.Query<User>().FirstOrDefaultAsync(o => o.Username == username);
        }
    }
}
