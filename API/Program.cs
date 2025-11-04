using Api.Configurations;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.AddConfigurations();
        builder.Services.AddInfrastructure(builder.Configuration);


        var app = builder.Build();

        app.UseInfrastructure(builder.Configuration);

        app.MapControllers().RequireAuthorization();
        app.UseStaticFiles();
        app.Run();
    }
}