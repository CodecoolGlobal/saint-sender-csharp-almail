namespace SaintSender.Core.Services
{
    using MailKit.Net.Pop3;
    using MailKit.Net.Smtp;
    using MailKit.Security;
    using MimeKit;
    using SaintSender.Core.Interfaces;
    using SaintSender.Core.Models;
    using System.Windows.Forms;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    public class OnlineMailerService : IMailerClient
    {
        public override void SendMail(EmailMessage email)
        {
            if (IsConnectedToInternet())
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(email.Sender, email.Sender));
                foreach (string address in email.Receiver)
                    emailMessage.To.Add(new MailboxAddress(address, address));

                emailMessage.Subject = email.Subject;
                emailMessage.Body = new TextPart("plain") { Text = email.Body };

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
                    // TODO: try to resend 3 times, and show message if this failed.
                }
            } //TODO else notify the user that the internet connection has been cut.
        }

        public override bool LoadMails(bool clearStorage = false)
        {
            SecureStorageAccess storageAccess = new SecureStorageAccess();

            Emails.Clear();

            if (!UserLoggedIn)
                return true;

            if (IsConnectedToInternet())
            {
                Debug.WriteLine("Getting emails from online source");

                if (!clearStorage)
                    Emails.AddRange(storageAccess.GetUserEmails(UserEmail));

                using (Pop3Client pop3Client = new Pop3Client())
                {
                    pop3Client.Connect("pop.gmail.com", 995, true);

                    try
                    {
                        pop3Client.Authenticate(UserEmail, UserPassword);
                    }
                    catch (AuthenticationException)
                    {
                        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        Match match = regex.Match(UserEmail);
                        if (match.Success)
                        {
                            MessageBox.Show("Invalid user credentials.");
                            LogOutCurrentUser();
                            return false;
                        }
                        else
                        {
                            MessageBox.Show("Incorrect email format.");
                            LogOutCurrentUser();
                            return false;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Unknown error");
                        LogOutCurrentUser();
                        return false;
                    }
                        
                    for (int i = 0; i < pop3Client.Count; i++)
                    {
                        var message = new EmailMessage(pop3Client.GetMessage(i));
                        message.IsSent = IsMessageTypeSent(message);
                        message.IsReceived = IsMessageTypeReceived(message);
                        if (!Emails.Contains(message))
                            Emails.Add(message);
                    }

                    pop3Client.Disconnect(true);
                    storageAccess.SaveUserEmails(UserEmail, Emails);

                    return true;
                }
            }
            else if (UserAccount.ValidateLoginCredentials(UserEmail, Hasher.Encode(UserPassword)))
            {
                Debug.WriteLine("Getting emails from offline source");
                Emails.AddRange(storageAccess.GetUserEmails(UserEmail));
                return true;
            }

            return false;
        }

        public override void ChangeEmailReadStatus(int emailIndex, bool status)
        {
            if (emailIndex < 0 || emailIndex >= Emails.Count)
                return;

            Emails[emailIndex].IsRead = status;

            if (!UserLoggedIn)
                return;

            SecureStorageAccess storageAccess = new SecureStorageAccess();
            storageAccess.SaveUserEmails(UserEmail, Emails);
        }
    }
}
