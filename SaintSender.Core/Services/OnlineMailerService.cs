namespace SaintSender.Core.Services
{
    using MailKit.Net.Pop3;
    using MailKit.Net.Smtp;
    using MimeKit;
    using SaintSender.Core.Interfaces;
    using SaintSender.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class OnlineMailerService : IMailerClient
    {
        public UserAccount account;

        private bool userIsLoggedIn = true;

        internal Pop3Client pop3Client = new Pop3Client();
        internal SmtpClient smtpClient = new SmtpClient();

        private static int currentPageShown = 1;

        private static int mailsPerPage = 24;

        public void LogInUser(string userEmail, string password)
        {
            //Offline validating
            /*if(account.ValidateLoginCredentials(userEmail, Hasher.Hash(password)))
            {
                account.Email = userEmail;
                userIsLoggedIn = true;
            } else
            {
                throw new Exception("Login data is incorrect.");
            }*/


            smtpClient.Connect("smtp.gmail.com", 587, true);
            smtpClient.Authenticate(userEmail, password);

            pop3Client.Connect("pop.gmail.com", 995, true);
            pop3Client.Authenticate(userEmail, password);
            Debug.WriteLine(GetMail());
            userIsLoggedIn = true;
        }

        public void LogOutCurrentUser()
        {
            account.Email = null;
            userIsLoggedIn = false;

            smtpClient.Disconnect(true);
            smtpClient.Dispose();

            pop3Client.Disconnect(true);
            pop3Client.Dispose();
        }

        public void SendMail(EmailMessage email)
        {
            if (userIsLoggedIn)
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(email.Sender, email.Sender));
                foreach (string address in email.Receiver)
                {
                    emailMessage.To.Add(new MailboxAddress(address, address));
                }
                emailMessage.Subject = email.Subject;
                emailMessage.Body = new TextPart("html") { Text = email.Subject };
                try
                {
                     smtpClient.Send(emailMessage);
                }
                catch (Exception ex) //todo add another try to send email
                {
                    throw ex;
                }
            }
        }

        public bool IsMessageTypeSent(EmailMessage message)
        {
           return message.Sender == account.Email;
        }

        public bool IsMessageTypeReceived(EmailMessage message)
        {
            return message.Receiver.Contains(account.Email);
        }

        public List<EmailMessage> GetMail()
        {

            if (userIsLoggedIn)
            {
                List<EmailMessage> mailList = new List<EmailMessage>();
                for (int i = (currentPageShown * mailsPerPage) - mailsPerPage; i < pop3Client.Count; i++)
                {
                    if (i == currentPageShown * mailsPerPage)
                    {
                        break;
                    }
                    var message = pop3Client.GetMessage(i);
                    mailList.Add(new EmailMessage(message));
                }
                return mailList;
            }
            else
            {
                throw new Exception("There is no active user.");
            }
        }
    }
}
