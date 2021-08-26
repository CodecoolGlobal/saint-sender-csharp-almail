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

        private static int currentPageShown = 1;

        private static int MAILS_PER_PAGE = 24;

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
           return message.Sender == account.Email;
        }

        public bool IsMessageTypeReceived(EmailMessage message)
        {
            return message.Receiver.Contains(account.Email);
        }

        public override bool LoadMails()
        {
            Emails.Clear();

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

                    for (int i = (currentPageShown * MAILS_PER_PAGE) - MAILS_PER_PAGE; i < pop3Client.Count; i++)
                    {
                        if (i == currentPageShown * MAILS_PER_PAGE)
                            break;

                        var message = pop3Client.GetMessage(i);
                        Emails.Add(new EmailMessage(message));
                    }

                    pop3Client.Disconnect(true);

                    return true;
                }

                // TODO: save all emails into the account's json file
            }
            else
                return true;

            return false;
        }
    }
}
