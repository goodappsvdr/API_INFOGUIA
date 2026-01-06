
using Api.Shared.Models;


using Api.Shared.DTOs.Auth;

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



        //public async Task<bool> LoginAsync(Auth_Login login)
        //{
        //	var usuario = await _context.Users
        //		.Where(x => x.Email == login.Username)
        //		.FirstOrDefaultAsync();

        //	// Verifica si el usuario fue encontrado
        //	if (usuario != null)
        //	{
        //		// Verifica las credenciales
        //		if (usuario.Email == login.Username && usuario.PasswordHash == login.Password)
        //		{
        //			return true; // Credenciales correctas
        //		}
        //	}

        //	return false; // Usuario no encontrado o credenciales incorrectas
        //}


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
                UserId = data.User.UserId,
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



        //public async Task<Jwt_Claims> GetClaimsAsync(string username)
        //{
        //    // 1️⃣ Usuario base
        //    User model = await _context.Users
        //        .FirstOrDefaultAsync(x => x.Email == username);

        //    if (model == null)
        //        return null!;

        //    // 2️⃣ Datos de rol + estado
        //    // Movimos la lógica de 'States' al WHERE para evitar el error de tipos en el JOIN
        //    var datos = await (from u in _context.Users
        //                       where u.Email == username
        //                       join ur in _context.VwAspnetUsersInRoles on (object)u.UserId equals (object)ur.UserId
        //                       join r in _context.VwAspnetRoles on ur.RoleId equals r.RoleId
        //                       from s in _context.States
        //                       where (u.IsActive ? 1 : 0) == s.StateId // Ajuste de comparación bool vs int
        //                       select new
        //                       {
        //                           RoleName = r.RoleName,
        //                           RoleId = r.RoleId,
        //                           StatusName = s.Name
        //                       }).FirstOrDefaultAsync();

        //    // 3️⃣ Mapeo de Claims base
        //    Jwt_Claims claim = _mapper.Map<Jwt_Claims>(model);

        //    if (datos != null)
        //    {
        //        claim.RoleName = datos.RoleName;
        //        claim.RoleId = datos.RoleId;
        //        claim.Status = datos.StatusName;
        //    }

        //    // 4️⃣ Sucursales
        //    var branchOffices = await (from us in _context.BranchsUsers
        //                               where us.UserId == model.UserId
        //                               join bo in _context.BranchsOffices on us.BranchOfficeId equals bo.BranchOfficeId
        //                               select new Jwt_Claims_BracnhOffice
        //                               {
        //                                   BranchId = bo.BranchOfficeId ?? 0,
        //                                   Description = bo.Name,
        //                                   PointSale = bo.SalesPoint
        //                               }).ToListAsync();

        //    claim.BranchOffices = branchOffices;

        //    return claim;
        //}
    }
}
