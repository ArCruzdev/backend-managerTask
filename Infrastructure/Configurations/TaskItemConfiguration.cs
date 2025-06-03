using Domain.Entities; // Para acceder a la entidad TaskItem
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("TaskItems"); // Nombre de la tabla
        builder.HasKey(ti => ti.Id);

        builder.Property(ti => ti.Title)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(ti => ti.Description)
            .HasMaxLength(2000); // Descripción más larga

        builder.Property(ti => ti.DueDate)
            .IsRequired();

        // Mapeo de enums a string
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
            .OnDelete(DeleteBehavior.Restrict); // No eliminar proyecto si tiene tareas

        // Relación: Un TaskItem puede ser asignado a un Usuario (opcional)
        builder.HasOne(ti => ti.AssignedToUser)
            .WithMany() // No necesitamos una colección de TaskItems en User por ahora, solo la relación
            .HasForeignKey(ti => ti.AssignedToUserId)
            .IsRequired(false) // Permite que AssignedToUserId sea NULL
            .OnDelete(DeleteBehavior.SetNull); // Si el usuario asignado se borra, la tarea queda sin asignar
    }
}