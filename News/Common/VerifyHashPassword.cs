using System.Security.Cryptography;

namespace News.Common
{
    public class VerifyHashPassword
    {
        private readonly PasswordHasherOptions options;
        private readonly BytesEqualityComparer comparer;

        public VerifyHashPassword(PasswordHasherOptions options, BytesEqualityComparer comparer)
        {
            this.options = options;
            this.comparer = comparer;
        }
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            if (hashedPasswordBytes.Length != options.HashSize + options.SaltSize)
            {
                return false;
            }

            byte[] hashBytes = new byte[options.HashSize];
            Buffer.BlockCopy(hashedPasswordBytes, 0, hashBytes, 0, options.HashSize);
            byte[] saltBytes = new byte[options.SaltSize];
            Buffer.BlockCopy(hashedPasswordBytes, options.HashSize, saltBytes, 0, options.SaltSize);

            byte[] providedHashBytes;
            using (var keyDerivation = new Rfc2898DeriveBytes(providedPassword, saltBytes, options.Iterations, options.HashAlgorithmName))
            {
                providedHashBytes = keyDerivation.GetBytes(options.HashSize);
            }

            return comparer.Equals(hashBytes, providedHashBytes);
        }
    }
}
