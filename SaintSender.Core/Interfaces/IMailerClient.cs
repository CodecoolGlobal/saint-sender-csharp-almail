using SaintSender.Core.Models;
using System.Collections.Generic;

namespace SaintSender.Core.Interfaces
{
    public interface IMailerClient
    {
        void LogInUser(string userEmail, string password);
        void LogOutCurrentUser();
        void SendMail(Models.EmailMessage email);
        List<EmailMessage> GetMail();
    }
}
