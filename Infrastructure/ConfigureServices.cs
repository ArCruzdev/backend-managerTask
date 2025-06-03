using Application.Common.Interfaces; // Para IApplicationDbContext
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Para leer la cadena de conexión
using Microsoft.Extensions.DependencyInjection;
using System.Reflection; // Para Assembly.GetExecutingAssembly()

namespace Infrastructure; 

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuración de la base de datos (SQL Server)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), // Obtenemos la cadena de conexión
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName))); // Especificamos la ubicación de las migraciones

        // Si usas PostgreSQL, sería así:
        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
        //         b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)));


        // Registrar IApplicationDbContext con su implementación ApplicationDbContext
        // AddScoped: Una instancia por cada petición HTTP
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        // Si tienes otros servicios de infraestructura (ej., servicios de correo, almacenamiento)
        // se registrarían aquí.
        // services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}