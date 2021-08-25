using SaintSender.Core.Models;
using System.Collections.Generic;

namespace SaintSender.Core.Interfaces
{
    public abstract class IMailerClient
    {
        public List<EmailMessage> Emails { get; protected set; } = new List<EmailMessage>();

        protected bool UserLoggedIn { get; private set; }
        protected string UserEmail { get; private set; }
        protected string UserPassword { get; private set; }

        public void LogInUser(string userEmail, string password)
        {
            UserEmail = userEmail;
            UserPassword = password;

            // elmentjük secure storagebe is

            UserLoggedIn = true;

            LoadMails();
        }
        public void LogOutCurrentUser()
        {
            UserEmail = UserPassword = null;
            UserLoggedIn = false;
        }
        public abstract void SendMail(Models.EmailMessage email);
        public abstract void LoadMails();
    }
}
