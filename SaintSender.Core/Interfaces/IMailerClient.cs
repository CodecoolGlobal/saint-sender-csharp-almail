using SaintSender.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace SaintSender.Core.Interfaces
{
    public abstract class IMailerClient
    {
        public List<EmailMessage> Emails { get; protected set; } = new List<EmailMessage>();

        protected bool UserLoggedIn { get; private set; }
        public string UserEmail { get; private set; }
        public string UserPassword { get; private set; }
        public bool IsConnectedToInternet()
        {
            string host = "www.youtube.com";
            Ping p = new Ping();
            try
            {
                PingReply reply = p.Send(host, 3000);
                if (reply.Status == IPStatus.Success)
                    return true;
            }
            catch { }
            return false;
        }

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
