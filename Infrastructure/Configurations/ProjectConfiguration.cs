using Domain.Entities; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // Configuración de la tabla y clave primaria
        builder.ToTable("Projects"); // Nombre de la tabla en la DB
        builder.HasKey(p => p.Id);   // Clave primaria

        // Configuración de propiedades
        builder.Property(p => p.Name)
            .HasMaxLength(200) // Longitud máxima
            .IsRequired();     // Obligatorio (NOT NULL)

        builder.Property(p => p.Description)
            .HasMaxLength(1000); // Opcional, pero con longitud máxima

        // Mapeo del enum ProjectStatus a una cadena de texto en la DB
        builder.Property(p => p.Status)
            .HasConversion<string>() // Guarda el enum como string
            .HasMaxLength(50)
            .IsRequired();

        // Relaciones: Un proyecto tiene muchas TaskItems
        builder.HasMany(p => p.Tasks)
            .WithOne(ti => ti.Project)
            .HasForeignKey(ti => ti.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); // Si se borra un proyecto, sus tareas también
       
    }
}