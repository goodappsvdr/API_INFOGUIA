namespace Api.Shared.DTOs.Emails
{
    public class Email_Register
    {
        public Email_Register(string email, string username, string url)
        {
            Email = email;
            Username = username;
            Url = url;
        }
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
