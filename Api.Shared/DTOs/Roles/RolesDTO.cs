namespace Api.Shared.DTOs.Roles
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ModifiedByUserId { get; set; }
    }

    public class CreateRoleDto
    {
        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? CreatedByUserId { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ModifiedByUserId { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public int? ModifiedByUserId { get; set; }
    }

}
