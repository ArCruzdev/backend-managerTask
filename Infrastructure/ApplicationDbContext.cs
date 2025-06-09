using Application.Common.Interfaces; 
using Domain.Entities; 
using Domain.Common; 
using Microsoft.EntityFrameworkCore;
using MediatR; 
using System.Reflection; 
using Infrastructure.Common; 

namespace Infrastructure; 

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator; 

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
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    // Sobrescribimos SaveChangesAsync para publicar los eventos de dominio
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Antes de guardar, procesamos los eventos de dominio. 
        await _mediator.DispatchDomainEvents(this); 

        // Guardar los cambios normales en la base de datos
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
