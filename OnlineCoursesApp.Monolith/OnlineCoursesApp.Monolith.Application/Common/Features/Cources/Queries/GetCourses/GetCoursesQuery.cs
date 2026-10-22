using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;

public record GetCoursesQuery(
    CourseCategory? Category,
    Guid? AuthorId,
    string? Search,
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null) : IRequest<PagedResult<CourseDto>>;
