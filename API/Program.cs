using Api.Configurations;
using Api.Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ==================================================================
// SERVICIOS
// ==================================================================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

builder.AddConfigurations();
builder.Services.AddInfrastructure(builder.Configuration);

// CORS
ConfigureCors(builder.Services, builder.Configuration);

var app = builder.Build();

// ==================================================================
// PIPELINE
// ==================================================================

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseCors("DefaultPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseInfrastructure(builder.Configuration);
app.MapControllers().RequireAuthorization();

app.Run();

// ==================================================================
// CONFIGURACIÓN DE CORS
// ==================================================================

static void ConfigureCors(IServiceCollection services, IConfiguration configuration)
{
    var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? Array.Empty<string>();

    services.AddCors(options =>
    {
        options.AddPolicy("DefaultPolicy", builder =>
        {
            if (allowedOrigins.Length == 0 || allowedOrigins.Contains("*"))
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }
            else
            {
                builder.WithOrigins(allowedOrigins)
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }
        });
    });
}