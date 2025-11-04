namespace Api.Shared.DTOs.Identity.Roles
{
    public class Role_Create
    {
        public Role_Create(string name)
        {
            Name = name;
            NormalizedName = name.ToUpper();
        }

        public string Name { get; set; } = default!;
        public string NormalizedName { get; set; } = default!;
    }
}
