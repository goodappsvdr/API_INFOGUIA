using System.Collections.ObjectModel;

namespace Api.Shared.DTOs.Identity.RoleClaims
{
    public class UpdateRoleClaims
    {
        public string RoleId { get; set; } = default!;
        public ICollection<string> Permissions { get; set; } = new Collection<string>();
    }
}
