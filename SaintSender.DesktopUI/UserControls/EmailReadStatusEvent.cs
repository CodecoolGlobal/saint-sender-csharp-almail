using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaintSender.DesktopUI.UserControls
{
    class EmailReadStatusEventArgs : EventArgs
    {
        public int EmailIndex { get; private set; }
        public bool EmailStatus { get; private set; }

        public EmailReadStatusEventArgs(int emailIndex, bool status)
        {
            EmailIndex = emailIndex;
            EmailStatus = status;
        }
    }
}
