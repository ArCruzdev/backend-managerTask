using Domain.Entities; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("TaskItems"); 
        builder.HasKey(ti => ti.Id);

        builder.Property(ti => ti.Title)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(2000); 

        builder.Property(ti => ti.DueDate)
            .IsRequired();

        
        builder.Property(ti => ti.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(ti => ti.Priority)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        // Relación: Un TaskItem pertenece a un Proyecto
        builder.HasOne(ti => ti.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(ti => ti.ProjectId)
            .OnDelete(DeleteBehavior.Restrict); 

        // Relación: Un TaskItem puede ser asignado a un Usuario 
        builder.HasOne(ti => ti.AssignedToUser)
            .WithMany() 
            .HasForeignKey(ti => ti.AssignedToUserId)
            .IsRequired(false) 
            .OnDelete(DeleteBehavior.SetNull); // Si el usuario asignado se borra, la tarea queda sin asignar
    }
}