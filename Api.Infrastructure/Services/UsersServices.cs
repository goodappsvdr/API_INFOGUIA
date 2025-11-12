using Api.Shared.DTOs.Identity.Roles;
using Api.Shared.Identity;
using Api.Shared.Interface;
using Api.Shared.Jwt;

namespace Api.Infrastructure.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IIdentityRepository<AspNetUser> _identityRepository;
        private readonly IFilesServices _filesServices;
        private readonly IMapper _mapper;
        public UsersServices(IIdentityRepository<AspNetUser> identityRepository, IFilesServices filesServices, IMapper mapper)
        {
            _identityRepository = identityRepository;
            _filesServices = filesServices;
            _mapper = mapper;
        }

        // Verifica si el usuario esta activo
        public async Task<bool> IsActive(string UserId) => _identityRepository.GetAsync(x => x.Id == UserId).Result.Active.Value;

        // Verifica si el usuario esta eliminado
        public async Task<bool> IsDeleted(string UserId) => _identityRepository.GetAsync(x => x.Id == UserId).Result.Delete.Value;

        // Verifica si el usuario esta verificado
        public async Task<bool> IsVerified(string UserId) => _identityRepository.GetAsync(x => x.Id == UserId).Result.EmailConfirmed;

        // Obtiene los claims del usuario
        public async Task<Jwt_Claims> GetClaimsAsync(string UsernName)
        {

            IdentityUserProfile userModel = await _identityRepository.GetIdentityUserByNameAsync(UsernName);

            //var roles = await _identityRepository.GetIdentityUserRolesAsync(userModel); // (Deberás crear este método en IdentityRepository)
                                                                                        // o modificar GetIdentityUserByNameAsync para que los incluya si es posible.
                                                                                        // Por ahora, lo mapeamos sin roles para avanzar.

            Jwt_Claims Claims = _mapper.Map<Jwt_Claims>(userModel);



            return Claims;
        }

        // Obtiene el modelo de IdentityUser por el nombre
        public async Task<IdentityUserProfile> GetIdentityUserByNameAsync(string Name) => await _identityRepository.GetIdentityUserByNameAsync(Name);

        // Obtiene el modelo de IdentityUser por el email
        public async Task<IdentityUserProfile> GetIdentityUserByEmailAsync(string Email) => await _identityRepository.GetIdentityUserByEmailAsync(Email);

        // Obtiene el modelo de IdentityUser por el id
        public async Task<IdentityUserProfile> GetIdentityUserByIdAsync(string Id) => await _identityRepository.GetIdentityUserByIdAsync(Id);

        // Agrega un nuevo usuario
        public async Task<IdentityResult> AddAsync(User_Create CreateModel)
        {
            try
            {
                IdentityUserProfile User = _mapper.Map<IdentityUserProfile>(CreateModel);
                User.Photo = "";
                User.Delete = false;
                User.Active = true;

                var createUserResult = await _identityRepository.AddUserAsync(User, CreateModel.Password);

                if (!createUserResult.Succeeded) return IdentityResult.Failed();

                var userRoleResult = await _identityRepository.AddRoleToUser(User, new Role_Create("User"));

                if(!userRoleResult.Succeeded) return IdentityResult.Failed();

                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }

        }

        // Modifica un usuario
        public async Task<IdentityResult> UpdateAsync(User_Update UpdateModel, Jwt_Claims Claims)
        {
            try
            {
                IdentityUserProfile User = await _identityRepository.GetIdentityUserByIdAsync(Claims.UserId.ToString());

                User.FirstName = UpdateModel.FirstName;
                User.LastName = UpdateModel.LastName;
                User.Notification = UpdateModel.Notification;
                User.CountryId = UpdateModel.CountryId;
                User.Lat = UpdateModel.Lat;
                User.Lng = UpdateModel.Lng;

                var UpdateUserResult = await _identityRepository.UpdateUserAsync(User);

                if (!UpdateUserResult.Succeeded) return UpdateUserResult;

                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }

        }

        // Actualiza la imagen de un usuario
        public async Task<IdentityResult> UpdateImage(IdentityUserProfile UserProfile, IFormFile Image)
        {
            try
            {

                if (UserProfile.Photo != "") await _filesServices.DeleteImage(UserProfile.Photo);

                UserProfile.Photo = await _filesServices.UploadImage(Image, "Users");

                if (UserProfile.Photo == "") return IdentityResult.Failed();

                var UpdateUserResult = await _identityRepository.UpdateUserAsync(UserProfile);

                if (!UpdateUserResult.Succeeded) return UpdateUserResult;

                return IdentityResult.Success;
            }
            catch
            {
                return IdentityResult.Failed();
            }
        }

        // Elimina la cuenta de un usuario
        public async Task<IdentityResult> DeleteAsync(IdentityUserProfile UserProfile)
        {
            UserProfile.Delete = true;
            return await _identityRepository.UpdateUserAsync(UserProfile);
        }

        // Elimina un usuario
        public async Task<IdentityResult> UserDeleteAsync(IdentityUserProfile UserProfile)
        {
            try
            {
                UserProfile.Delete = true;
                UserProfile.LockoutEnabled = false;
                UserProfile.LockoutEnd = DateTimeOffset.MaxValue;

                var UpdateUserResult = await _identityRepository.UpdateUserAsync(UserProfile);

                if (!UpdateUserResult.Succeeded) return UpdateUserResult;

                return IdentityResult.Success;

            }
            catch
            {
                return IdentityResult.Failed();
            }

        }

        // Genera el token de verificación de correo electrónico
        public async Task<string> GenerateValidateTokenByEmail(string Email)
        {
            IdentityUserProfile user = await _identityRepository.GetIdentityUserByEmailAsync(Email);

            if (user == null) return null;

            var Token = await _identityRepository.GenerateEmailConfirmationTokenAsync(user);
            var TokenUrl = Base64UrlEncoder.Encode(Token);

            user.ConfirmEmailToken = Token;
            await _identityRepository.UpdateUserAsync(user);
            return TokenUrl;
        }

        // Genera el token de cambio de contraseña
        public async Task<string> GenerateChangePasswordTokenByEmail(string Email)
        {
            IdentityUserProfile user = await _identityRepository.GetIdentityUserByEmailAsync(Email);

            if (user == null) return null;

            var Token = await _identityRepository.GeneratePasswordResetTokenAsync(user);
            var TokenUrl = Base64UrlEncoder.Encode(Token);
            user.ChangePasswordToken = Token;
            await _identityRepository.UpdateUserAsync(user);
            return TokenUrl;
        }

        // Valida el correo electronico con el token
        public async Task<IdentityUserProfile> ValidateEmailByToken(string Token)
        {
            var TokenDecode = Base64UrlEncoder.Decode(Token);

            var UserId = _identityRepository.GetAsync(x => x.ConfirmEmailToken == TokenDecode).Result.Id;

            if (UserId == null) return null;

            var user = await _identityRepository.GetIdentityUserByIdAsync(UserId);

            if (user == null) return null;

            var result = await _identityRepository.ConfirmEmailAsync(user, TokenDecode);

            if (!result.Succeeded) return null;

            user.EmailConfirmed = true;
            user.ConfirmEmailToken = "";
            await _identityRepository.UpdateUserAsync(user);
            return user;
        }

        // Cambia la contraseña con el token
        public async Task<IdentityUserProfile> ChangePasswordByToken(string Password, string Token)
        {

            var TokenDecode = Base64UrlEncoder.Decode(Token);

            var UserId = _identityRepository.GetAsync(x => x.ChangePasswordToken == TokenDecode).Result.Id;

            if (UserId == null) return null;

            var user = await _identityRepository.GetIdentityUserByIdAsync(UserId);

            if (user == null) return null;

            await _identityRepository.ResetPasswordAsync(user, TokenDecode, Password);

            user.ChangePasswordToken = "";
            await _identityRepository.UpdateUserAsync(user);
            return user;
        }

        // Cambia la contraseña utilizando la anterior
        public async Task<IdentityResult> ChangePasswordAsync(IdentityUserProfile User, User_ChangePassword Model) => await _identityRepository.ChangePasswordAsync(User, Model);

    }
}
