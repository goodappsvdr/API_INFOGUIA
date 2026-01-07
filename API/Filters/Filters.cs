using Api.Infrastructure.Jwt;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class JwtAuthorizationAttribute : TypeFilterAttribute
    {
        public JwtAuthorizationAttribute() : base(typeof(JwtAuthorizationFilter))
        {
        }
    }

    public class JwtAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Authorization header is missing or invalid." });
                return;
            }

            var token = authHeader.Replace("Bearer ", "");
            var userId = Jwt_Helpers.GetIdUserByToken(token);

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid or expired token." });
                return;
            }

            // Guardamos el UserId en HttpContext para usarlo en los controladores
            context.HttpContext.Items["UserId"] = userId;
        }
    }
}
