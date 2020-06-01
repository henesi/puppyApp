using AuthService.Domain;
using AuthService.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain
{
    public class User
    {
        public virtual Guid? UserId { get; protected set; }
        public virtual string Username { get; protected set; }
        public virtual string Email { get; protected set; }
        public virtual byte[] PasswordHash { get; protected set; }
        public virtual byte[] PasswordSalt { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual int Role { get; protected set; }

        protected User() { } //NH required

        public User(
            string userName,
            string email,
            string password)
        {
            var crypt = PasswordCrypt.CreatePasswordHash(password);
            UserId = null;
            Username = userName;
            Email = email;
            CreationDate = DateTime.Now;
            PasswordHash = crypt.passwordHash;
            PasswordSalt = crypt.passwordSalt;
            Role = (int)Roles.SUPERUSER;
        }
    }
}
