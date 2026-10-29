using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("courses");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(5000).IsRequired();

        builder.Property(c => c.Price)
            .HasColumnType("numeric(18, 2)")
            .IsRequired();
        builder.Property(c => c.Category)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(c => c.Category);
        builder.HasIndex(c => c.AuthorId);

        builder.HasOne(c => c.Author)
            .WithMany(u => u.AuthoredCourses)
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Materials)
            .WithOne(m => m.Course)
            .HasForeignKey(m => m.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}