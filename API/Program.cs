using Api.Configurations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.AddConfigurations();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddOutputCache(opciones => {
            opciones.AddPolicy("entidades", builder => builder.Expire(TimeSpan.FromHours(4)).Tag("entidades"));
            opciones.AddPolicy("entidadesrecibos", builder => builder.Expire(TimeSpan.FromSeconds(60)).Tag("entidadesrecibos"));
            opciones.AddPolicy("categorias", builder => builder.Expire(TimeSpan.FromHours(12)).Tag("categorias"));
            opciones.AddPolicy("estados", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("estados"));
            opciones.AddPolicy("parametros", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("parametros"));
            opciones.AddPolicy("documentocliente", builder=> builder.Expire(TimeSpan.FromSeconds(60)).Tag("documentocliente"));
            opciones.AddPolicy("documentoproveedor", builder=> builder.Expire(TimeSpan.FromSeconds(60)).Tag("documentoproveedor"));
            opciones.AddPolicy("provincias", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("provincias"));
            opciones.AddPolicy("localidades", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("localidades"));
            opciones.AddPolicy("tarjetas", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("tarjetas"));
            opciones.AddPolicy("listaprecio", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("listaprecio"));
            opciones.AddPolicy("cuentabanacaria", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("cuentabanacaria"));
            opciones.AddPolicy("bancos", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("bancos"));
            opciones.AddPolicy("bancosucursales", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("bancosucursales"));
            opciones.AddPolicy("items", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("items"));
            opciones.AddPolicy("impuestos", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("impuestos"));
            opciones.AddPolicy("notificaciones", builder=> builder.Expire(TimeSpan.FromHours(12)).Tag("notificaciones"));
		});



        var app = builder.Build();
        app.UseOutputCache();
        app.UseInfrastructure(builder.Configuration);

        app.MapControllers().RequireAuthorization();

        app.Run();
    }
}