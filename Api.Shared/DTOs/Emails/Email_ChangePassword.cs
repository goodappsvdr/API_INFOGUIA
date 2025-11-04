namespace Api.Shared.DTOs.Emails
{
    public class Email_ChangePassword
    {
        public Email_ChangePassword(string email, string url)
        {
            Email = email;
            Url = url;
        }
        public string Email { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
