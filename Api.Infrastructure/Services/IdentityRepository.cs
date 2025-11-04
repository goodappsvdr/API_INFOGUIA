using Api.Shared.DTOs.Identity.Roles;

namespace Api.Infrastructure.Repository
{
    public class IdentityRepository<T> : Repository<T>, IIdentityRepository<T> where T : class
    {
        private readonly UserManager<IdentityUserProfile> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUserProfile> _signInManager;
        public IdentityRepository(UserManager<IdentityUserProfile> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUserProfile> signInManager, Context context) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        // Funcion para el Login
        public async Task<SignInResult> LoginAsync(Auth_Login login) => await _signInManager.PasswordSignInAsync(login.Username, login.Password, isPersistent: false, lockoutOnFailure: false);

        // Obtiene el modelo de IdentityUser por el nombre
        public async Task<IdentityUserProfile> GetIdentityUserByNameAsync(string Name) => await _userManager.FindByNameAsync(Name);

        // Obtiene el modelo de IdentityUser por el email
        public async Task<IdentityUserProfile> GetIdentityUserByEmailAsync(string Email) => await _userManager.FindByEmailAsync(Email);

        // Obtiene el modelo de IdentityUser por el id
        public async Task<IdentityUserProfile> GetIdentityUserByIdAsync(string Id) => await _userManager.FindByIdAsync(Id);

        // Agrega un nuevo usuario
        public async Task<IdentityResult> AddUserAsync(IdentityUserProfile Model, string Password)
        {
            try
            {
                var createUserResult = await _userManager.CreateAsync(Model, Password);

                if (!createUserResult.Succeeded)
                {
                    await DeleteUserAsync(Model);
                    return IdentityResult.Failed();
                }

                return IdentityResult.Success;
            }
            catch
            {

                return IdentityResult.Failed();
            }

        }

        // Agrega un rol a un usuario
        public async Task<IdentityResult> AddRoleToUser(IdentityUserProfile Model, Role_Create role_Create)
        {
            try
            {
                await AddRoleAsync(new IdentityRole
                {
                    Name = role_Create.Name,
                    NormalizedName = role_Create.NormalizedName,
                });

                await _userManager.AddToRoleAsync(Model, role_Create.Name);
                return IdentityResult.Success;
            }
            catch
            {

                return IdentityResult.Failed();
            }
        }

        // Agrega un nuevo Rol
        public async Task<IdentityResult> AddRoleAsync(IdentityRole Model)
        {
            if (await _roleManager.RoleExistsAsync(Model.Name) == false) await _roleManager.CreateAsync(Model);
            return IdentityResult.Success;
        }

        // Modifica un usuario
        public async Task<IdentityResult> UpdateUserAsync(IdentityUserProfile Model)
        {
            try
            {
                var UpdateUserResult = await _userManager.UpdateAsync(Model);

                if (!UpdateUserResult.Succeeded) return UpdateUserResult;

                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }

        }

        // Elimina la cuenta de un usuario
        public async Task<IdentityResult> DeleteUserAsync(IdentityUserProfile UserProfile) => await _userManager.DeleteAsync(UserProfile);

        // Elimina un rol
        public async Task<IdentityResult> DeleteRoleAsync(IdentityRole UserProfile) => await _roleManager.DeleteAsync(UserProfile);

        // Cambia la contraseña utilizando la anterior
        public async Task<IdentityResult> ChangePasswordAsync(IdentityUserProfile User, User_ChangePassword Model) => await _userManager.ChangePasswordAsync(User, Model.CurrenPassword, Model.NewPassword);

        // Genera un Token para verificar el correo electronico
        public async Task<string> GenerateEmailConfirmationTokenAsync(IdentityUserProfile User) => await _userManager.GenerateEmailConfirmationTokenAsync(User);

        // Genera un Tokem para cambiar la contraseña
        public async Task<string> GeneratePasswordResetTokenAsync(IdentityUserProfile User) => await _userManager.GeneratePasswordResetTokenAsync(User);

        // Confirma que el token enviado al correo sea correcto
        public async Task<IdentityResult> ConfirmEmailAsync(IdentityUserProfile User, string Token) => await _userManager.ConfirmEmailAsync(User, Token);

        // Cambia la contraseña utlizando el token
        public async Task<IdentityResult> ResetPasswordAsync(IdentityUserProfile User, string Token, string NewPassword) => await _userManager.ResetPasswordAsync(User, Token, NewPassword);
    }
}
