using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;

public class GetCoursesQueryHandler: IRequestHandler<GetCoursesQuery, PagedResult<CourseDto>>
{
    private readonly ICourseRepository _courseRepository;

    public GetCoursesQueryHandler(
        ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<PagedResult<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var filter = new CourseFilter(
            request.Category,
            request.AuthorId,
            request.Search,
            request.Page,
            request.PageSize,
            request.SortBy);

        var result = await _courseRepository.GetFilteredAsync(filter, cancellationToken);

        var items = result.Items
            .Select(c => new CourseDto(c.Id, c.Title, c.Description,
                c.AuthorId, c.Category, c.Price, c.CreatedAt))
            .ToList();

        return new PagedResult<CourseDto>
            (items, result.Page, result.PageSize, result.TotalCount);
    }
}