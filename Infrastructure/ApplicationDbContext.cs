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
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> TaskItems => Set<TaskItem>(); 
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // Antes de guardar, procesamos los eventos de dominio. 
        await _mediator.DispatchDomainEvents(this); 

        // Guardar los cambios normales en la base de datos
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
