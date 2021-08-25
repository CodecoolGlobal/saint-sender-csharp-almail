using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.Core.Interfaces
{
    interface IMailerClient
    {
        void LogInUser(string userEmail, string password);
        void LogOutCurrentUser();
        void SendMail(Models.EmailMessage email);
        void GetMail();
    }
}
