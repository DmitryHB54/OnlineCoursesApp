using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;

public class GetCoursesQueryHandler: IRequestHandler<GetCoursesQuery, PagedResult<CourseDto>>
{
    private readonly ICourseFilteredReadRepository _courseFiltereReadRepository;
    private readonly IMapper _mapper;

    public GetCoursesQueryHandler(
        ICourseFilteredReadRepository courseFiltereReadRepository,
        IMapper mapper)
    {
        _courseFiltereReadRepository = courseFiltereReadRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<CourseDto>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var filter = _mapper.Map<CourseFilter>(request);

        var result = await _courseFiltereReadRepository.GetFilteredAsync(filter, cancellationToken);

        return result;
    }
}