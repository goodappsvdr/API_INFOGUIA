namespace Api.Shared.DTOs.Identity.UserRoles
{
    public class UserRoles
    {
        public string? IdRole { get; set; } = default!;

        public string? RoleName { get; set; } = default!;

        public bool Enabled { get; set; } = default!;
    }
}
