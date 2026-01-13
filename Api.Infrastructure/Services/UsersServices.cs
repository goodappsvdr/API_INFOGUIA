
using Api.Shared.Models;


using Api.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Api.Infrastructure.Services
{
	public class UsersServices : IUsersServices
	{
		private readonly ContextInfoGuia _context;
		private readonly IMapper _mapper;

		public UsersServices(ContextInfoGuia context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

         
        public async Task<bool> LoginAsync(Auth_Login login)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Email.ToLower() == login.Username.ToLower() &&
                    x.IsActive == true);

            if (user == null)
                return false;

            return user.PasswordHash == login.Password;
        }

        public async Task<Jwt_Claims> GetClaimsAsync(string username)
        {
            var data = await (
                from u in _context.Users.AsNoTracking()
                join r in _context.Roles.AsNoTracking()
                    on u.RoleId equals r.RoleId
                where u.Email == username && u.IsActive
                select new
                {
                    User = u,
                    RoleName = r.Name,
                    RoleId = r.RoleId
                }
            ).FirstOrDefaultAsync();

            if (data == null)
                return null;

            Jwt_Claims claim = new Jwt_Claims
            {
                UserId = data.User.UserId ,
                Email = data.User.Email,
                FirstName = data.User.FirstName,
                LastName = data.User.LastName,
                TenantId = data.User.TenantId,
                RoleId = data.RoleId,
                RoleName = data.RoleName,
                Status = data.User.IsActive ? "Activo" : "Inactivo"
            };

            claim.BranchOffices = await (
                from us in _context.BranchsUsers
                join bo in _context.BranchsOffices
                    on us.BranchOfficeId equals bo.BranchOfficeId
                where us.UserId == data.User.UserId
                select new Jwt_Claims_BracnhOffice
                {
                    BranchId = bo.BranchOfficeId ?? 0,
                    Description = bo.Name,
                    PointSale = bo.SalesPoint
                }
            ).ToListAsync();

            return claim;
        }

    }
}
