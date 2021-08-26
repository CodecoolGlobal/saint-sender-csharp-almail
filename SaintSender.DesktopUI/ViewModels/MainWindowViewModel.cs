using SaintSender.Core.Interfaces;
using SaintSender.Core.Models;
using SaintSender.Core.Services;

namespace SaintSender.DesktopUI.ViewModels
{
    /// <summary>
    /// ViewModel for Main window. Contains all shown information
    /// and necessary service classes to make view functional.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private IMailerClient _mailService = new OnlineMailerService();

        /// <summary>
        /// Authenticates a user with email and password
        /// </summary>
        /// <param name="email">Email address</param>
        /// <param name="password">Password as text</param>
        public bool LogIn(string email, string password)
        {
            return _mailService.LogInUser(email, password);
        }

        /// <summary>
        /// Logs out the logged in user
        /// </summary>
        public void LogOut()
        {
            _mailService.LogOutCurrentUser();
        }

        /// <summary>
        /// Refreshes the list of emails
        /// </summary>
        public void RefreshMails()
        {
            _mailService.LoadMails();
        }

        public void UpdateEmailReadStatus(int emailIndex, bool readStatus)
        {
            _mailService.ChangeEmailReadStatus(emailIndex, readStatus);
        }

        /// <summary>
        /// Returns an EmailMessage array
        /// </summary>
        /// <returns>an EmailMessage array</returns>
        public EmailMessage[] GetEmailList()
        {
            return _mailService.Emails.ToArray();
        }

        public void SendMail(string receiver, string subject, string body)
        {
            EmailMessage emailMessage = new EmailMessage(_mailService.UserEmail, receiver, subject, body, body);
            _mailService.SendMail(emailMessage);
        }
    }
}
