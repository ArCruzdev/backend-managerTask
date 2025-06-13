using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // DbSet for each of your main entities (aggregate roots)
    DbSet<Project> Projects { get; }
    DbSet<TaskItem> TaskItems { get; }
    DbSet<User> Users { get; }
    DbSet<Comment> Comments { get; }

    // Method to save changes asynchronously
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}