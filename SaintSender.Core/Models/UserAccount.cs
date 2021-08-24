namespace SaintSender.Core.Models

{
    internal struct UserAccount
    {
        public UserAccount(string name, string hashedPassword)
        {
            Email = name;
            HashedPassword = hashedPassword;
        }

        public string Email { get; private set; }

        public string HashedPassword { get; set; }
    }
}