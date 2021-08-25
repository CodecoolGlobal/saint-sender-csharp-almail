
using System.Collections.Generic;
using MimeKit;
using System.Diagnostics;

namespace SaintSender.Core.Models
{
    public class EmailMessage

    {
        public EmailMessage(string sender, string receiver, string subject, string body)
        {
            Sender = sender;
            Receiver.Add(receiver);
            Subject = subject;
            Body = body;
        }

        public EmailMessage(MimeMessage message)
        {
            //Debug.WriteLine(message);
            foreach (var mailbox in message.From.Mailboxes)
            {
                Sender = mailbox.Address;
            }
            foreach (var mailbox in message.To.Mailboxes)
            {
                Receiver.Add(mailbox.Address);
            }
            SentTime = message.Date.DateTime;
            Subject = message.Subject;
            Body = message.GetTextBody(MimeKit.Text.TextFormat.Text);
        }


        public System.DateTime SentTime { get; }
        public string Sender { get; }
        public List<string> Receiver { get; set; } = new List<string>();

        public string Subject { get; }
        public string Body { get; }
    }


    public static class InternetAddressListExtension{

        public static List<string> getAddressList(this InternetAddressList list)
        {
            List<string> returnList = new List<string>();
            foreach (InternetAddress address in list)
            {
                returnList.Add(address.Name);
            }
            return returnList;
        }
    }
}
