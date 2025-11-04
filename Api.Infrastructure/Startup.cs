
using Api.Shared.Identity;
using Api.Shared.Interface;
using Microsoft.AspNetCore.Http.Connections;

namespace Api.Infrastructure;
public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient();
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddSqlConnection(config);
        services.AddIdentitySettings();
        services.AddJwtTokenServices(config);
        services.AddCorsSettings();
        services.AddSignalRSettings();
        services.AddAutoMapperSettings();
        services.AddMailing(config);
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
        app.UseSwaggerGen();
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
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
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
                   policy.WithOrigins("http://localhost:5173")
                         .AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials()
                         .SetIsOriginAllowed(origin => true);
               });
        });
        return services;
    }
    internal static IServiceCollection AddServicesSettings(this IServiceCollection services)
    {
        services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient(typeof(IIdentityRepository<>), typeof(IdentityRepository<>));

        services.AddTransient<IUsersServices, UsersServices>();
        services.AddTransient<IAuthServices, AuthServices>();
        services.AddTransient<IFilesServices, FilesServices>();
        services.AddTransient<IEmailServices, EmailServices>();
        services.AddTransient<INotificationsServices, NotificationsServices>();
        services.AddTransient<IPoliticsServices, PoliticsServices>();
        services.AddTransient<IHelpServices, HelpServices>();
        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
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
    internal static IServiceCollection AddIdentitySettings(this IServiceCollection services)
    {
        services.AddIdentity<IdentityUserProfile, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 2;
            options.Password.RequiredUniqueChars = 0;
            //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //options.Lockout.MaxFailedAccessAttempts = 5;
            //options.Lockout.AllowedForNewUsers = true;
        });

        return services;
    }
    internal static IServiceCollection AddMailing(this IServiceCollection services, IConfiguration config) => services.Configure<MailSettings>(config.GetSection(nameof(MailSettings)));
    internal static IServiceCollection AddAutoMapperSettings(this IServiceCollection services) => services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());
}
