using Application; // Para AddApplicationServices
using Infrastructure; // Para AddInfrastructureServices
using Microsoft.EntityFrameworkCore; // Para Database.Migrate()
using Infrastructure; // Para ApplicationDbContext (si lo necesitas para migraciones)
using Infrastructure.Common; // Para MediatorExtensions (si lo usas en el contexto de la app)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ******************************************************************************
// ******************* INYECCI�N DE DEPENDENCIAS DE NUESTRAS CAPAS *************
// ******************************************************************************

// Inyectar servicios de la capa de Aplicaci�n
builder.Services.AddApplicationServices();

// Inyectar servicios de la capa de Infraestructura
builder.Services.AddInfrastructureServices(builder.Configuration);

// ******************************************************************************

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            // Puedes configurar or�genes espec�ficos desde appsettings.json
            var allowedOrigins = builder.Configuration.GetValue<string>("CorsOrigins")?.Split(',');

            if (allowedOrigins != null && allowedOrigins.Any())
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials(); // Si manejas credenciales (cookies, auth headers)
            }
            else
            {
                policy.AllowAnyOrigin() // SOLO para desarrollo o APIs p�blicas sin autenticaci�n
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ******************************************************************************
// ******************* APLICACI�N DE MIGRACIONES AL INICIO (para desarrollo) ***
// ******************************************************************************

// Esta es la parte para aplicar migraciones autom�ticamente al iniciar.
// �til en desarrollo, pero en producci�n se recomienda un contenedor de migraciones dedicado.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Solo aplica si el proveedor de la DB es SQL Server (o Npgsql si usas PostgreSQL)
        if (context.Database.IsSqlServer()) // Cambia a IsNpgsql() si usas PostgreSQL
        {
            app.Logger.LogInformation("Applying database migrations...");
            context.Database.Migrate(); // Aplica las migraciones pendientes
            app.Logger.LogInformation("Database migrations applied successfully.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        // Considerar pol�ticas de reintento o reintentar el inicio del contenedor en Docker Compose
    }
}

// ******************************************************************************

app.UseHttpsRedirection();
app.UseCors(); // Aseg�rate de que UseCors est� antes de UseAuthorization

app.UseAuthorization();

app.MapControllers();

app.Run();
