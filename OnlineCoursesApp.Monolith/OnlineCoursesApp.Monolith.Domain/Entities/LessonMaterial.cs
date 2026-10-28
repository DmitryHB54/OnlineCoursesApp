using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Domain.Entities;

public class LessonMaterial : BaseEntity
{
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public string Title { get; set; } = default!;
    public uint OrderNumber { get; set; }
    public MaterialType MaterialType { get; set; }

    public FileInfo FileInfo { get; set; } = default!;
}