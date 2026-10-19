using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasData(
            new Role { Id = (int)RoleName.Admin, Name = RoleName.Admin },
            new Role { Id = (int)RoleName.Teacher, Name = RoleName.Teacher },
            new Role { Id = (int)RoleName.Student, Name = RoleName.Student }
        );
    }
}