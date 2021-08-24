namespace SaintSender.Core.Models

{
    internal struct UserAccount
    {
        public UserAccount(string email, string hashedPassword)
        {
            Email = email;
        }

        public string Email { get; private set; }
        //TODO: Save password to isolated storage
    }
}