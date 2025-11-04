namespace Api.Infrastructure.Identity
{
    public class IdentityUserProfile : IdentityUser
    {
        public int? CountryId { get; set; }

        public string? Photo { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? Notification { get; set; }

        public bool? Active { get; set; }

        public bool? Delete { get; set; }

        public double? Lat { get; set; }

        public double? Lng { get; set; }

        public string? RefreshToken { get; set; }

        public string? ChangePasswordToken { get; set; }

        public string? ConfirmEmailToken { get; set; }
    }
}
