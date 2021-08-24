
namespace SaintSender.Core.Models
{
    struct Email
    {
        public Email(UserAccount sender, UserAccount receiver, string subject, string body)
        {
            Sender = sender;
            Receiver = receiver;
            Subject = subject;
            Body = body;
        }

        public UserAccount Sender { get; set; }
        public UserAccount Receiver { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
