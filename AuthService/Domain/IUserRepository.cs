using AuthService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User> WithUsername(string username);
    }
}
