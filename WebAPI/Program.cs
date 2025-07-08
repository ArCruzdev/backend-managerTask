using Application; 
using Infrastructure; 
using Microsoft.EntityFrameworkCore; 
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyectar servicios de la capa de Aplicación
builder.Services.AddApplicationServices();

// Inyectar servicios de la capa de Infraestructura
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            var allowedOrigins = builder.Configuration.GetValue<string>("CorsOrigins")?.Split(',');

            if (allowedOrigins != null && allowedOrigins.Any())
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
            else
            {
                policy.AllowAnyOrigin() 
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
        var message = "Ha ocurrido un error inesperado en el servidor."; // Mensaje genérico para 500

        // Manejo específico excepciones personalizadas
        if (exception is NotFoundException notFoundEx)
        {
            statusCode = StatusCodes.Status404NotFound;
            message = notFoundEx.Message; 
        }
        else if (exception is BadRequestException badRequestEx) 
        {
            statusCode = StatusCodes.Status400BadRequest;
            message = badRequestEx.Message; 
        }
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        // Escribe el mensaje de error en el cuerpo de la respuesta JSON
        await context.Response.WriteAsJsonAsync(new { message = message });
    });
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsSqlServer()) 
        {
            app.Logger.LogInformation("Applying database migrations...");
            context.Database.Migrate(); 
            app.Logger.LogInformation("Database migrations applied successfully.");
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}

app.UseHttpsRedirection();
app.UseCors(); 

app.UseAuthorization();

app.MapControllers();

app.Run();
