namespace SaintSender.Core.Services
{
    using MailKit.Net.Pop3;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;
    using SaintSender.Core.Interfaces;
    using SaintSender.Core.Models;
    using System.Windows.Forms;

    public class OnlineMailerService : IMailerClient
    {
        public UserAccount account;

        public override void SendMail(EmailMessage email)
        {
            if (UserLoggedIn)
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(email.Sender, email.Sender));

                foreach (string address in email.Receiver)
                    emailMessage.To.Add(new MailboxAddress(address, address));

                emailMessage.Subject = email.Subject;
                emailMessage.Body = new TextPart("html") { Text = email.Subject };

                try
                {
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        smtpClient.Connect("smtp.gmail.com", 465, true);
                        smtpClient.Authenticate(UserEmail, UserPassword);
                        smtpClient.Send(emailMessage);
                    }
                }
                catch
                {
                    // TODO: try resend 3 times, and message show if its failed
                }
            }
        }

        public bool IsMessageTypeSent(EmailMessage message)
        {
           return message.Sender == UserEmail;
        }

        public bool IsMessageTypeReceived(EmailMessage message)
        {
            return message.Receiver.Contains(UserEmail);
        }

        public override bool LoadMails()
        {
            Emails.Clear();
            SecureStorageAccess storageAccess = new SecureStorageAccess();

            Emails.AddRange(storageAccess.GetUserEmails(UserEmail));
            if (UserLoggedIn)
            {
                using (Pop3Client pop3Client = new Pop3Client())
                {
                    pop3Client.Connect("pop.gmail.com", 995, true);

                    try
                    {
                        pop3Client.Authenticate(UserEmail, UserPassword);
                    }
                    catch (AuthenticationException)
                    {
                        MessageBox.Show("Invalid user credentials.");
                        LogOutCurrentUser();
                        return false;
                    }
                    catch
                    {
                        MessageBox.Show("Unknown error");
                        LogOutCurrentUser();
                        return false;
                    }

                    for (int i = 0 ; i < pop3Client.Count; i++)
                    {
                        var message = new EmailMessage(pop3Client.GetMessage(i));
                        if (!Emails.Contains(message))
                            Emails.Add(message);
                    }
                    
                    pop3Client.Disconnect(true);
                    storageAccess.SaveUserEmails(UserEmail, Emails);
                    return true;
                }
            }
            else
                return true;
        }
    }
}
