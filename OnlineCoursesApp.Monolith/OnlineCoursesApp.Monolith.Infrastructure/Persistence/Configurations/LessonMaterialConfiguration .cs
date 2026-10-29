using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Configurations;

public class LessonMaterialConfiguration : IEntityTypeConfiguration<LessonMaterial>
{
    public void Configure(EntityTypeBuilder<LessonMaterial> builder)
    {
        builder.ToTable("lesson_materials");
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Title).HasMaxLength(300).IsRequired();
        builder.Property(m => m.OrderNumber).IsRequired();
        builder.Property(m => m.MaterialType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(m => new { m.CourseId, m.OrderNumber }).IsUnique();

        builder.OwnsOne(m => m.FileInfo, fi =>
        {
            fi.Property(fi => fi.RelativePath)
                .HasColumnName("file_relative_path")
                .HasMaxLength(500)
                .IsRequired();

            fi.Property(f => f.OriginalName)
                .HasColumnName("file_original_name")
                .HasMaxLength(500)
                .IsRequired();

            fi.Property(f => f.SizeBytes)
                .HasColumnName("file_size_bytes")
                .IsRequired();

            fi.Property(f => f.Extension)
                .HasColumnName("file_extension")
                .HasMaxLength(20)
                .IsRequired();
        });
    }
}