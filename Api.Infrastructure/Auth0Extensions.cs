using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Infrastructure
{
    public static class Auth0Extensions
    {
        public static IServiceCollection AddAuth0Authentication(this IServiceCollection services, IConfiguration config)
        {
            var domain = config["Auth0:Domain"];
            var audience = config["Auth0:Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = audience;
            });

            return services;
        }
    }
}
