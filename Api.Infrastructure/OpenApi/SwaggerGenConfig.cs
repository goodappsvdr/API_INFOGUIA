using Api.Infrastructure.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Api;

public static class SwaggerGenConfig
{
    internal static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

        // Validación mejorada
        if (settings?.Enable != true)
            return services;

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            // Información general de la API
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = settings.Title ?? "API",
                Version = settings.Version ?? "v1",
                Description = settings.Description,
                Contact = CreateContactInfo(settings),
                License = CreateLicenseInfo(settings)
            });

            // Configuración de seguridad JWT
            ConfigureJwtSecurity(options);

            // Incluir comentarios XML
            IncludeXmlComments(options);

            // Mejoras adicionales
            ConfigureAdditionalOptions(options);
        });

        return services;
    }

    internal static IApplicationBuilder UseSwaggerGen(this IApplicationBuilder app, IConfiguration config)
    {
        var settings = config.GetSection(nameof(SwaggerSettings)).Get<SwaggerSettings>();

        if (settings?.Enable != true)
            return app;

        app.UseSwagger(options =>
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        });

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", $"{settings.Title} {settings.Version}");
            options.RoutePrefix = "swagger"; // URL: /swagger

            // Mejoras de UI
            options.DocExpansion(DocExpansion.None);
            options.DefaultModelsExpandDepth(-1); // Oculta schemas por defecto
            options.EnableValidator(null);
            options.DisplayRequestDuration(); // Muestra tiempo de respuesta
            options.EnableDeepLinking(); // Permite enlaces directos
            options.EnableFilter(); // Habilita búsqueda
            options.ShowExtensions();

            // Personalización visual
            options.DocumentTitle = settings.Title ?? "API Documentation";
            options.InjectStylesheet("/swagger-custom/custom-styles.css"); // Si tienes CSS custom
        });

        return app;
    }

    #region Private Helper Methods

    private static OpenApiContact? CreateContactInfo(SwaggerSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.ContactName) &&
            string.IsNullOrWhiteSpace(settings.ContactEmail))
            return null;

        return new OpenApiContact
        {
            Name = settings.ContactName,
            Email = settings.ContactEmail,
            Url = TryCreateUri(settings.ContactUrl)
        };
    }

    private static OpenApiLicense? CreateLicenseInfo(SwaggerSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.LicenseName))
            return null;

        return new OpenApiLicense
        {
            Name = settings.LicenseName,
            Url = TryCreateUri(settings.LicenseUrl)
        };
    }

    private static Uri? TryCreateUri(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;

        return Uri.TryCreate(url, UriKind.Absolute, out var uri) ? uri : null;
    }

    private static void ConfigureJwtSecurity(SwaggerGenOptions options)
    {
        // Definición de seguridad
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = @"Autorización JWT usando el esquema Bearer.
                <br/><br/>Ingresa tu token en el campo de texto a continuación.
                <br/><br/>Ejemplo: <code>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</code>
                <br/><br/><strong>No es necesario agregar 'Bearer' al inicio</strong>"
        });

        // Requerimiento de seguridad
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    }

    private static void IncludeXmlComments(SwaggerGenOptions options)
    {
        try
        {
            // Buscar todos los archivos XML en el directorio base
            var xmlFiles = new[]
            {
                "Api.xml",                           // Tu proyecto principal
                "Api.Shared.xml",                    // Si tienes un proyecto Shared
                "Api.Infrastructure.xml"             // Si tienes un proyecto Infrastructure
            };

            foreach (var xmlFile in xmlFiles)
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            }
        }
        catch (Exception ex)
        {
            // Log pero no fallar si no encuentra los XML
            Console.WriteLine($"Warning: Could not load XML documentation: {ex.Message}");
        }
    }

    private static void ConfigureAdditionalOptions(SwaggerGenOptions options)
    {
        // Ordenar acciones alfabéticamente
        options.OrderActionsBy(apiDesc =>
            $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

        // Personalizar IDs de operaciones
        options.CustomOperationIds(apiDesc =>
        {
            var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"];
            var actionName = apiDesc.ActionDescriptor.RouteValues["action"];
            return $"{controllerName}_{actionName}";
        });

        // Usar nombres descriptivos para enums
        options.UseInlineDefinitionsForEnums();

        // Filtros personalizados (si los necesitas)
        // options.OperationFilter<AddResponseHeadersFilter>();
        // options.SchemaFilter<EnumSchemaFilter>();
    }

    #endregion
}