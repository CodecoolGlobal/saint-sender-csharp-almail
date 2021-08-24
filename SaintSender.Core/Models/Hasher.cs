using System.Security.Cryptography;
using System.Text;
using System;


namespace SaintSender.Core.Models
{
    public class Hasher
    {
        private static string HashKey = "UC80Dp2qRF70PVoCinjwp6QegPMKCyZZfd3FNulJceIfqHzVUSCIcRfhJNYh";

        public string Hash(string stringToHash)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(HashKey);
            byte[] passwordBytes = encoding.GetBytes(stringToHash);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                hmacsha256.hash
                byte[] hashmessage = hmacsha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
