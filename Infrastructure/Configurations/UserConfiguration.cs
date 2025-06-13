using Domain.Entities; 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(250)
            .IsRequired();
        builder.HasIndex(u => u.Email) 
            .IsUnique();

        builder.Property(u => u.Username)
            .HasMaxLength(100)
            .IsRequired();
        builder.HasIndex(u => u.Username) 
            .IsUnique();

        builder.Property(u => u.Role)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired();

        // Relación: Un usuario puede tener muchos comentarios
        builder.HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}