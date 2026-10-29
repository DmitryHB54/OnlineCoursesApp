using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public RoleName Name { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();
}