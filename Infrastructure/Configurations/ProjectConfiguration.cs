// Path: Infrastructure/Configurations/ProjectConfiguration.cs
using Domain.Entities; // Para acceder a la entidad Project
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

        // Relaciones: Un proyecto tiene muchos Usuarios (miembros del proyecto)
        // Esto asume una relación de muchos a muchos o una tabla intermedia si es necesario.
        // Por simplicidad, si la lista de miembros en Project es solo una colección de Users directamente,
        // EF Core puede intentar una tabla de unión implícita.
        // Si no tienes una entidad ProjectMember, EF Core creará una tabla de unión por defecto.
        // Si tuvieras una entidad ProjectMember, sería así:
        // builder.HasMany(p => p.ProjectMembers)
        //     .WithOne(pm => pm.Project)
        //     .HasForeignKey(pm => pm.ProjectId);
    }
}