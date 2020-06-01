using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Domain
{
    public class PasswordCrypt
    {
        public static Password CreatePasswordHash(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                return new Password
                {
                    passwordSalt = hmac.Key,
                    passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password))
                };
            }
        }
        public static bool VerifyPasswordHash(string password, byte[] storedSalt, byte[] storedHash)
        {
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var passwordHashActual = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < storedHash.Length; i++)
                {
                    if (passwordHashActual[i] != storedHash[i]) return false;
                }
            }
            return true;
        }
    }

    public class Password
    {
        public virtual byte[] passwordHash { get; set; }
        public virtual byte[] passwordSalt { get; set; }

    }
}
