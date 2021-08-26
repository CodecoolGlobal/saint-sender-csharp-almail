using System.Collections.Generic;

namespace SaintSender.Core.Models
{
    public struct UserAccount
    {
        public UserAccount(string email, string hashedPassword)
        {
            Email = email;
            HashedPassword = hashedPassword;
            SecureStorageAccess storageAccess = new SecureStorageAccess();
            storageAccess.SaveUser(email, hashedPassword);
        }

        public bool ValidateLoginCredentials(string email, string hashedPassword)
        {
            SecureStorageAccess storageAccess = new SecureStorageAccess();
            Dictionary<string,string> userData = storageAccess.GetUserLoginData(email);
            return userData[email] == hashedPassword;
        }

        public string Email { get; set; }
        public string HashedPassword { get; private set; }

    }
}