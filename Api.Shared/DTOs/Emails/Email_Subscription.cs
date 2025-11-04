namespace Api.Shared.DTOs.Emails
{
    public class Email_Subscription
    {
        public Email_Subscription(string email, string username)
        {
            Email = email;
            Username = username;
        }

        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
