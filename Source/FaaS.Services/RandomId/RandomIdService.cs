using System;
using System.Security.Cryptography;

namespace FaaS.Services.RandomId
{
    public class RandomIdService : IRandomIdService
    {
        public string GenerateRandomString(int entropyBits)
        {
            int numberOfBytesNeeded = (entropyBits + 7) / 8;

            var bytes = new Byte[numberOfBytesNeeded];
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(bytes);
            }

            return FromBase64ToBase64Url(Convert.ToBase64String(bytes));
        }

        private string FromBase64ToBase64Url(string base64)
        {
            return base64.TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}
