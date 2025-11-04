namespace Api.Shared.DTOs.Identity.UserRoles
{
    public class CreateOrUpdateUserRoles
    {
        public ICollection<UserRoles> UserRoles { get; set; } = default!;
    }
}
