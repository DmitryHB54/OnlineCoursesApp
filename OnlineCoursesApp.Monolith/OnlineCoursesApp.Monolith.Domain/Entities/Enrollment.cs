namespace OnlineCoursesApp.Monolith.Domain.Entities;

public class Enrollment
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid CourseId { get; set; }
    public Course Course { get; set; } = default!;

    public DateTimeOffset EnrolledAt { get; set; }
}