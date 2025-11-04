using Api.Infrastructure.OpenApi;

namespace Api;
public static class SwaggerGenConfig
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration config)
    {

        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

        if (settings == null) return services;

        if (settings.Enable)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = settings.Title,
                    Version = settings.Version,
                    Description = settings.Description,
                    Contact = new OpenApiContact
                    {
                        Name = settings.ContactName,
                        Email = settings.ContactEmail,
                        Url = new Uri(settings.ContactUrl!),
                    },
                    License = new OpenApiLicense
                    {
                        Name = settings.LicenseName,
                        Url = new Uri(settings.LicenseUrl!)
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Autorizacion JWT esquema. Escribe el token proporcionado.",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                             Reference = new OpenApiReference
                             {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                             },
                        },
                       new string[]{}
                    }
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Api.xml"));
            });
        }

        return services;
    }
    internal static IApplicationBuilder UseSwaggerGen(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.EnableValidator(null);
            c.DocExpansion(DocExpansion.None);
        });
        return app;
    }
}
