using Api.Shared.Jwt;

namespace Api.Infrastructure.Jwt
{
    public static class Jwt_AddJwtTokenServicesExtensions
    {
        public static void AddJwtTokenServices(this IServiceCollection services, IConfiguration configuration)
        {

            Jwt_RefreshTokenSettings bindRefreshTokenSettings = new();
            configuration.Bind("RefreshTokenSettings", bindRefreshTokenSettings);
            services.AddSingleton(bindRefreshTokenSettings);

            Jwt_AccessTokenSettings bindAccessTokenSettings = new();
            configuration.Bind("AccessTokenSettings", bindAccessTokenSettings);
            services.AddSingleton(bindAccessTokenSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddCookie(x =>
            {
                x.Cookie.Name = "AccessToken";
            })
                .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindAccessTokenSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bindAccessTokenSettings.IssuerSigningKey)),
                    ValidateIssuer = bindAccessTokenSettings.ValidateIssuer,
                    ValidIssuer = bindAccessTokenSettings.ValidIssuer,
                    ValidateAudience = bindAccessTokenSettings.ValidateAudience,
                    ValidAudience = bindAccessTokenSettings.ValidAudience,
                    RequireExpirationTime = bindAccessTokenSettings.RequireExpirationTime,
                    ValidateLifetime = bindAccessTokenSettings.ValidateLifeTime,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["AccessToken"];
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization();
        }

    }
}
