
using System;
using System.Collections.Generic;
using MimeKit;
using Newtonsoft.Json;

namespace SaintSender.Core.Models
{
    [Serializable]
    public class EmailMessage
    {
        public EmailMessage(string sender, string receiver, string subject, string body)
        {
            Sender = sender;
            Receiver.Add(receiver);
            Subject = subject;
            Body = body;
        }


        [JsonConstructor]
        public EmailMessage(string sentTime ,string sender, string[] receiver, string subject, string body)
        {
            SentTime = DateTime.Parse(sentTime);
            Sender = sender;
            Receiver.AddRange(receiver);
            Subject = subject;
            Body = body;
        }
        public EmailMessage(MimeMessage message)
        {
            foreach (var mailbox in message.From.Mailboxes)
                Sender = mailbox.Address;

            foreach (var mailbox in message.To.Mailboxes)
                Receiver.Add(mailbox.Address);

            SentTime = message.Date.DateTime;
            Subject = message.Subject;
            Body = message.GetTextBody(MimeKit.Text.TextFormat.Text);
        }

        public System.DateTime SentTime { get; }

        public string Sender { get; }

        public List<string> Receiver { get; set; } = new List<string>();

        public string Subject { get; }

        public string Body { get; }

        public override string ToString()
        {
            return "Sender: " + Sender + "\nReceiver: " + Receiver + "\nSubject: " + Subject + "\nBody: " + Body + "\nSentTime: " + SentTime;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            try
            {
                EmailMessage otherMessage = obj as EmailMessage;
                if (otherMessage != null && otherMessage.Sender.Equals(Sender) &&
                    System.Linq.Enumerable.SequenceEqual(otherMessage.Receiver, Receiver) &&
                    otherMessage.Body.Equals(Body) &&
                    otherMessage.Subject.Equals(Subject) &&
                    otherMessage.SentTime.Ticks == SentTime.Ticks)
                {
                    return true;
                }
                else
                    return false;

            }
            catch { 
                return false;
            }
        }
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
