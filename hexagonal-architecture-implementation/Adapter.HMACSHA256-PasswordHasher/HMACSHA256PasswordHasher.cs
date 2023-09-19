using Domain.Ports.Driven;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Adapter.HMACSHA256_PasswordHasher
{
    public class HMACSHA256PasswordHasher : IPasswordHasher
    {
        private string _salt;
        public HMACSHA256PasswordHasher(string salt)
        {
            _salt = salt;
        }
        public string Hash(string password)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Encoding.UTF8.GetBytes(_salt),
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));

            return hashedPassword;
        }
    }
}
