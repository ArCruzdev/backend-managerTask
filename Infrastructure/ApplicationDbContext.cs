using Application.Common.Interfaces; // Para IApplicationDbContext
using Domain.Entities; // Para acceder a nuestras entidades de dominio
using Domain.Common; // Para BaseEntity y BaseEvent
using Microsoft.EntityFrameworkCore;
using MediatR; // Para usar IMediator para publicar eventos de dominio
using System.Reflection; // Para ApplyConfigurationsFromAssembly
using Infrastructure.Common; // Para el método de extensión DispatchDomainEvents

namespace Infrastructure; 

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator; // Inyectamos IMediator para eventos de dominio

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    // Definimos los DbSet para nuestras entidades principales
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>(); 
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    // Este método se llama cuando se crea el modelo de la base de datos.
    // Aquí configuraremos el mapeo de nuestras entidades.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplicar configuraciones de entidad desde este ensamblado.
        // Crearemos estas configuraciones explícitas en una nueva carpeta "Configurations".
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Si planeas usar ASP.NET Core Identity más adelante, descomentarías esto:
        // base.OnModelCreating(modelBuilder);
    }

    // Sobrescribimos SaveChangesAsync para publicar los eventos de dominio
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Antes de guardar, procesamos los eventos de dominio. Esto es crucial para DDD.
        await _mediator.DispatchDomainEvents(this); // Este método de extensión lo crearemos a continuación.

        // Guardar los cambios normales en la base de datos
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
