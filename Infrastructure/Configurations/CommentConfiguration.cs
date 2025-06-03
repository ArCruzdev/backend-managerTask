// Path: Infrastructure/Configurations/CommentConfiguration.cs
using Domain.Entities; // Para acceder a la entidad Comment
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Text)
            .HasMaxLength(500)
            .IsRequired();

        // Relación: Un comentario pertenece a un TaskItem
        builder.HasOne(c => c.TaskItem)
            .WithMany(ti => ti.Comments)
            .HasForeignKey(c => c.TaskItemId)
            .OnDelete(DeleteBehavior.Cascade); // Si se borra la tarea, sus comentarios también

        // Relación: Un comentario es hecho por un Usuario
        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments) // Relación inversa en User (si existe)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); // No eliminar el usuario si tiene comentarios
    }
}