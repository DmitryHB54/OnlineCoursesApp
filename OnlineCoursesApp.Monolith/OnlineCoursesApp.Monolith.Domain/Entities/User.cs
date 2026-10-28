namespace OnlineCoursesApp.Monolith.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; } = default!;

    public ICollection<Course> AuthoredCourses { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}