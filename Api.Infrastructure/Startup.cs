//using Api.Infrastructure.Services.AccountBank;
using Api.Infrastructure.Services.Listings;
using Api.Shared.Data;
using Api.Shared.ServiciosExternos;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        services.AddDbContext<ContextInfoGuia>(options => options.UseSqlServer(connectionString));
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
        
        services.AddTransient<IUsersServices, UsersServices>();
        services.AddTransient<IListingsServices, ListingsServices>();
        //services.AddTransient<IStatusServices, StatusServices>();
        //services.AddTransient<IProvinceServices, ProvinceServices>();
        //services.AddTransient<ILocalityService, LocalityService>();
        //services.AddTransient<ICategoryServices, CategoryServices>();
        //services.AddTransient<IParameterServices, ParameterServices>();
        //services.AddTransient<IBankServices, BankServices>();
        //services.AddTransient<IBranchBankServices,BranchBankServices>();
        //services.AddTransient<IClientDocumentServices, ClientDocumentServices>();
        //services.AddTransient<ICurrentAccountServices, CurrentAccountServices>();
        //services.AddTransient<IPaymentElementServices, PaymentElementServices>();
        //services.AddTransient<IAccountBankServices, AccountBankServices>();
        //services.AddTransient<ITaxServices, TaxServices>();
        //services.AddTransient<IPriceListServices, PriceListServices>();
        //services.AddTransient<IItemsServices, ItemsServices>();
        //services.AddTransient<IRubroServices, RubroServices>();
        //services.AddTransient<ISubRubroServices, SubRubroServices>();
        //services.AddTransient<IBrandServices, BrandServices>();
        //services.AddTransient<IModelServices, ModelServices>();
        //services.AddTransient<IProviderDocumentServices, ProviderDocumentServices>();
        //services.AddTransient<IPayOrderServices,PayOrderServices>();
        //services.AddTransient<ICardsServicios, CardsServicios>();
        //services.AddTransient<IStaticConfigs, StaticConfigs>();
        //services.AddTransient<IProductSystelService, ProductSystelService>();
        //services.AddTransient<IReceiptServices, ReceiptServices>();
        //services.AddTransient<IPricelistVersionService, PricelistVersionService>();
        //services.AddTransient<IInvoiceServices, InvoiceServices>();
        //services.AddTransient<IDepartamentService, DepartamentServices>();
        //services.AddTransient<IFileServices , FileServices>();
        //services.AddTransient<IEntityServices , EntityServices>();

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
