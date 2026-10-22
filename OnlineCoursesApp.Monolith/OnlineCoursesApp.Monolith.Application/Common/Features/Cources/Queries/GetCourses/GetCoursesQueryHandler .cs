using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;

public class GetCoursesQueryHandler(
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<GetCoursesQuery, PagedResult<CourseDto>>
{
    public async Task<PagedResult<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var filter = new CourseFilter(
            request.Category,
            request.AuthorId,
            request.Search,
            request.Page,
            request.PageSize,
            request.SortBy);

        PagedResult<Course> result = await courseRepository.GetFilteredAsync(filter, cancellationToken);

        IReadOnlyList<CourseDto> items = result.Items
            .Select(c => new CourseDto(c.Id, c.Title, c.Description,
                c.AuthorId, c.Category, c.Price, c.CreatedAt))
            .ToList();

        return new PagedResult<CourseDto>
            (items, result.Page, result.PageSize, result.TotalCount);
    }
}