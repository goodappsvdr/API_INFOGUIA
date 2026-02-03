using Api.Infrastructure.Mapping;
using Api.Infrastructure.Services.Categories;
//using Api.Infrastructure.Services.Dynamic;

//using Api.Infrastructure.Services.Dynamic;
using Api.Infrastructure.Services.Listings;
using Api.Infrastructure.Services.ListingImages;
using Api.Infrastructure.Services.Roles;
using Api.Shared.Data;
using Api.Shared.ServiciosExternos;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Api.Infrastructure.Services.ListingImagesServices;

namespace Api.Infrastructure;
public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSqlConnection(config);
        services.AddJwtTokenServices(config);
        services.AddCorsSettings();
        services.AddSignalRSettings();
        services.AddAutoMapperSettings();
        services.AddServicesSettings();
        services.AddSwagger(config);
        return services;
    }
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration config)
    {
    
        app.UseStaticFiles();
        app.UseRouting();
        app.UseHttpsRedirection();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();
        app.UseSignalR();
        app.UseSwaggerGen(config);

        return app;
    }
    internal static IApplicationBuilder UseSignalR(this IApplicationBuilder app)
    {
        app.UseResponseCompression();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<NotificationHub>("/notificationHub", options =>
            {
                options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;

            });

            //endpoints.MapHub<ChatHub>("/chatHub", options =>
            //{
            //    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
            //});
        });
        return app;
    }
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapControllers();
        return builder;
    }
    internal static IServiceCollection AddSqlConnection(this IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("SQL");
        services.AddDbContext<Context>(options => options.UseSqlServer(connectionString));
        return services;
    }
    internal static IServiceCollection AddCorsSettings(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
			

			options.AddPolicy("CorsPolicy",
				policy =>
				{
					policy.AllowAnyOrigin()
						  .AllowAnyHeader()
						  .AllowAnyMethod();
				});
		});
        return services;
    }
    internal static IServiceCollection AddServicesSettings(this IServiceCollection services)
    {
        // Servicios existentes
        services.AddTransient<IUsersServices, UsersServices>();
        services.AddTransient<IListingsServices, ListingsServices>();
        services.AddTransient<IListingImagesServices, ListingImagesServices>();
        services.AddTransient<ICategorieServices, CategorieServices>();
        services.AddTransient<IRolesServices, RolesServices>();

        // ✅ AGREGAR: Servicios del sistema dinámico ABM
        //services.AddScoped<IDynamicModuleService, DynamicModuleService>();
        //services.AddScoped<IDynamicEntityService, DynamicEntityService>();
        //services.AddScoped<IDynamicDatabaseService, DynamicDatabaseService>();
        //services.AddScoped<IDynamicMappingService, DynamicMappingService>();

        // ✅ AGREGAR: AutoMapper profile si no lo tienes ya registrado
        services.AddAutoMapper(typeof(DynamicModuleProfile));

        return services;
    }

    internal static IServiceCollection AddSignalRSettings(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddResponseCompression(o =>
        {
            o.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/octet-stream" });
        });
        return services;
    }

    internal static IServiceCollection AddAutoMapperSettings(this IServiceCollection services) => services.AddAutoMapper(typeof(AutoMapperProfile));
}
