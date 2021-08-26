
using System;
using System.Collections.Generic;
using MimeKit;
using Newtonsoft.Json;

namespace SaintSender.Core.Models
{
    [Serializable]
    public class EmailMessage
    {
        public EmailMessage(EmailMessage copy)
        {
            HTMLBody = copy.HTMLBody;
            Sender = copy.Sender;
            Receiver.AddRange(copy.Receiver);
            Subject = copy.Subject;
            Body = copy.Body;
            if (Body == null)
                Body = "";
            SentTime = copy.SentTime;
            IsRead = copy.IsRead;
            IsSent = copy.IsSent;
            IsReceived = copy.IsReceived;
        }

        public EmailMessage(string sender, string receiver, string subject, string body, string html, bool isReaded = false, bool isSent = false, bool isReceived = false)
        {
            SentTime = DateTime.Now;
            HTMLBody = html;
            Sender = sender;
            Receiver.Add(receiver);
            Subject = subject;
            Body = body;
            if (Body == null)
                Body = "";
            IsRead = isReaded;
            IsSent = isSent;
            IsReceived = isReceived;
        }


        [JsonConstructor]
        public EmailMessage(string sentTime ,string sender, string[] receiver, string subject, string body, string html, bool isRead, bool isSent, bool isReceived)
        {
            SentTime = DateTime.Parse(sentTime);
            Sender = sender;
            Receiver.AddRange(receiver);
            Subject = subject;
            Body = body;
            if (Body == null)
                Body = "";
            HTMLBody = html;
            IsRead = isRead;
            IsSent = isSent;
            IsReceived = isReceived;
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
            if (Body == null)
                Body = "";

            HTMLBody = message.HtmlBody;
        }

        public EmailMessage Clone()
        {
            return new EmailMessage(this);
        }

        public System.DateTime SentTime { get; }

        public string Sender { get; }

        public List<string> Receiver = new List<string>();

        public string Subject;

        public bool IsRead;

        public string Body;

        public string HTMLBody { get; }

        public bool IsSent = false;

        public bool IsReceived = false;

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

                if (otherMessage == null)
                    return false;

                if (!otherMessage.Sender.Equals(Sender))
                    return false;
                if (!System.Linq.Enumerable.SequenceEqual(otherMessage.Receiver, Receiver))
                    return false;

                if (otherMessage.Body == null || Body == null || !Body.Equals(otherMessage.Body))
                    return false;

                if (!otherMessage.Subject.Equals(Subject))
                    return false;

                if (otherMessage.SentTime.Ticks != SentTime.Ticks)
                    return false;

                if (otherMessage.IsRead != IsRead || otherMessage.IsSent != IsSent)
                    return false;

                return true;
            }
            catch
            { 
                return false;
            }
        }

        public bool Compare(EmailMessage other)
        {
            if (other == null)
                return false;

            if (other.Sender != Sender)
                return false;

            if (other.SentTime.Ticks != SentTime.Ticks)
                return false;

            return true;
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
