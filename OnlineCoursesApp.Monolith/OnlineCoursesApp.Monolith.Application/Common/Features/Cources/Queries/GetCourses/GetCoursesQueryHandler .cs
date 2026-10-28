using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;

public class GetCoursesQueryHandler: IRequestHandler<GetCoursesQuery, PagedResult<CourseDto>>
{
    private readonly ICourseFilteredReadRepository _courseFiltereReadRepository;

    public GetCoursesQueryHandler(
        ICourseFilteredReadRepository courseFiltereReadRepository)
    {
        _courseFiltereReadRepository = courseFiltereReadRepository;
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

        var result = await _courseFiltereReadRepository.GetFilteredAsync(filter, cancellationToken);

        var items = result.Items
            .Select(c => new CourseDto(c.Id, c.Title, c.Description,
                c.AuthorId, c.Category, c.Price, c.CreatedAt))
            .ToList();

        return new PagedResult<CourseDto>
            (items, result.Page, result.PageSize, result.TotalCount);
    }
}