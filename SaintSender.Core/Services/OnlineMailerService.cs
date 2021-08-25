﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.Core.Models
{
    class MailerService : Interfaces.IMailerClient
    {
        public UserAccount account;
        private bool userIsLoggedIn = false;



        public void LogInUser(string userEmail, string password)
        {
            if(account.ValidateLoginCredentials(userEmail, Hasher.Hash(password)))
            {
                account.Email = userEmail;
                userIsLoggedIn = true;
            } else
            {
                throw new Exception("Login data is incorrect.");
            }
        }

        public void LogOutCurrentUser()
        {
            account.Email = null;
            userIsLoggedIn = false;
        }

        public void SendMail(EmailMessage email) {
            if (userIsLoggedIn)
            {

            }
        }
        public void GetMail()
        {
            throw new NotImplementedException();
        }
    }
}
