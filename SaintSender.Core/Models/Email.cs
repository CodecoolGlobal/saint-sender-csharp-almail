
namespace SaintSender.Core.Models
{
    struct Email
    {
        public UserAccount Sender { get; set; }
        public UserAccount Receiver { get; set; }

        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
