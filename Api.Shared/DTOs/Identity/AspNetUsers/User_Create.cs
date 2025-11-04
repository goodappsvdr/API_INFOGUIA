namespace Api.Shared.DTOs.Identity.AspNetUsers
{
    public class User_Create
    {
        public int CountryId { get; set; } = 0;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public bool Notification { get; set; } = false;

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
