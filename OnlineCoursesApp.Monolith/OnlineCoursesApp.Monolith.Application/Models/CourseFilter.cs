using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Models;

public record CourseFilter(
    CourseCategory? Category,
    Guid? AuthorId,
    string? Search,
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null);