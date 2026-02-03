namespace Api.Infrastructure.Jwt
{
    public class Jwt_Claims
    {
        public int UserId { get; set; } = default!;

        public int BranchId { get; set; } = default!;

        public string? Username { get; set; } = default!;

        public string? FirstName { get; set; } = default!;

        public string? Email { get; set; } = default!;

        public string? Photo { get; set; } = default!;

        public string RoleName { get; set; } = default!;

        public int? RoleId { get; set; } = default!;

        public string? Status { get; set; } = default!;
        public string? LastName { get; set; } = default!;
        public int? TenantId { get; set; } = default!;
        public string ImgProfile { get; set; }

        public List<Jwt_Claims_BracnhOffice> BranchOffices { get; set; } = default!;

    }

    public class  Jwt_Claims_BracnhOffice
    {
        

		public int BranchId { get; set; } = default!;

		public string? Description { get; set; } = default!;

        public string PointSale { get; set; } = default!;

        
    }
}
