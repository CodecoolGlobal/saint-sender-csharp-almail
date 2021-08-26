using System.Security.Cryptography;
using System.Text;
using System;


namespace SaintSender.Core.Models
{
    public class Hasher
    {
        private static string HashKey = "UC80Dp2qRF70PVoCinjwp6QegPMKCyZZfd3FNulJceIfqHzVUSCIcRfhJNYh";

        public static string Hash(string stringToHash)
        {
            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(HashKey);
            byte[] passwordBytes = encoding.GetBytes(stringToHash);

            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        public static string Encode (string textToEncode)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(textToEncode);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashKey));
            objMD5CryptoService.Clear();

            TripleDESCryptoServiceProvider objTripleDESCryptoService = new TripleDESCryptoServiceProvider
            {
                Key = securityKeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decode(string stringToDecode)
        {
            byte[] toEncryptArray = Convert.FromBase64String(stringToDecode);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(HashKey));
            objMD5CryptoService.Clear();
            TripleDESCryptoServiceProvider objTripleDESCryptoService = new TripleDESCryptoServiceProvider
            {
                Key = securityKeyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
