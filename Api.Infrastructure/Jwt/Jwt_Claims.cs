namespace Api.Infrastructure.Jwt
{
    public class Jwt_Claims
    {
        public string? UserId { get; set; } = default!;

        public string? CountryId { get; set; } = default!;

        public string? Username { get; set; } = default!;

        public string? FirstName { get; set; } = default!;

        public string? LastName { get; set; } = default!;

        public string? Email { get; set; } = default!;

        public string? Phone { get; set; } = default!;

        public string? Photo { get; set; } = default!;

        public string RoleName { get; set; } = default!;

        //public string Expiration { get; set; } = default!;
    }
}
