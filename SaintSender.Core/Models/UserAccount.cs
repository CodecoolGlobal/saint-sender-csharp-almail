using System.Collections.Generic;

namespace SaintSender.Core.Models
{
    public struct UserAccount
    {
        public UserAccount(string email, string encryptedPassword)
        {
            Email = email;
            EncryptedPassword = encryptedPassword;
            SecureStorageAccess storageAccess = new SecureStorageAccess();
            storageAccess.SaveUser(email, EncryptedPassword);
        }

        public static bool ValidateLoginCredentials(string email, string encryptedPassword)
        {
            SecureStorageAccess storageAccess = new SecureStorageAccess();
            Dictionary<string,string> userData = storageAccess.GetUserLoginData(email);
            try { return userData[email] == encryptedPassword; } catch { return false; }  
        }

        public string Email { get; set; }
        public string EncryptedPassword { get; private set; }
    }
}