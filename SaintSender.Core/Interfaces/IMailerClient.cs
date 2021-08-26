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

        /// <summary>
        /// Logs in an user with login credentials and refreshes the email list
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="password">User password</param>
        public bool LogInUser(string userEmail, string password)
        {
            UserEmail = userEmail;
            UserPassword = password;

            // TODO: elmentjük secure storagebe is

            UserLoggedIn = true;

            return LoadMails();
        }

        /// <summary>
        /// Logs out the currently logged in user
        /// </summary>
        public void LogOutCurrentUser()
        {
            UserEmail = UserPassword = null;
            UserLoggedIn = false;
            Emails.Clear();

            LoadMails();
        }

        /// <summary>
        /// Send an email message
        /// </summary>
        /// <param name="email">The email message</param>
        public abstract void SendMail(Models.EmailMessage email);

        /// <summary>
        /// (Re)loads the email list
        /// </summary>
        public abstract bool LoadMails();
    }
}
