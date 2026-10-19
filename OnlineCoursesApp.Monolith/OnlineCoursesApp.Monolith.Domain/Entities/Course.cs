using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Domain.Entities;
public class Course
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;

    public Guid AuthorId { get; set; }
    public User Author { get; set; } = default!;

    public CourseCategory Category { get; set; }
    public decimal Price { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<LessonMaterial> Materials = new List<LessonMaterial>();
    public ICollection<Enrollment> Enrollments = new List<Enrollment>();
}