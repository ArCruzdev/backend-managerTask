using Application; // Para AddApplicationServices
using Infrastructure; // Para AddInfrastructureServices
using Microsoft.EntityFrameworkCore; // Para Database.Migrate()
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

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

// --- Middleware Global de Manejo de Excepciones ---
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        var statusCode = StatusCodes.Status500InternalServerError;
        var message = "Ha ocurrido un error inesperado en el servidor."; // Mensaje gen�rico para 500

        // Manejo espec�fico de tus excepciones personalizadas
        if (exception is NotFoundException notFoundEx)
        {
            statusCode = StatusCodes.Status404NotFound;
            message = notFoundEx.Message; // Muestra el mensaje de la excepci�n de NotFound
        }
        else if (exception is BadRequestException badRequestEx) // <--- �ESTE ES EL BLOQUE A�ADIDO/MODIFICADO!
        {
            statusCode = StatusCodes.Status400BadRequest;
            message = badRequestEx.Message; // Muestra el mensaje de la excepci�n de BadRequest
        }
        // Puedes a�adir m�s bloques 'else if' para otros tipos de excepciones
        // Por ejemplo, para DomainException si tus entidades lanzan errores de negocio directamente.
        // else if (exception is DomainException domainEx) { ... }


        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        // Escribe el mensaje de error en el cuerpo de la respuesta JSON
        await context.Response.WriteAsJsonAsync(new { message = message });
    });
});
// --- FIN Middleware Global de Manejo de Excepciones ---

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
