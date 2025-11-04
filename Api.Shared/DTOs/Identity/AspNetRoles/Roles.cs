namespace Api.Shared.DTOs.Identity.Roles
{
    public class Roles
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public ICollection<string>? Permissions { get; set; } = default!;
    }
}
