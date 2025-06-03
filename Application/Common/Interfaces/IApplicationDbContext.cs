using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // DbSet para cada una de tus entidades principales (raíces de agregación)
    DbSet<Project> Projects { get; }
    DbSet<TaskItem> TaskItems { get; }
    DbSet<User> Users { get; }
    DbSet<Comment> Comments { get; }

    // Método para guardar los cambios de forma asíncrona
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}