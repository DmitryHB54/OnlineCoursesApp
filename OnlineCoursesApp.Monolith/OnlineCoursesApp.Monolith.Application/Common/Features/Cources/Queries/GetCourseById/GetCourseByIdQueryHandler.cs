using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseById;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public GetCourseByIdQueryHandler(
        ICourseRepository courseRepository,
        IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken)
           ?? throw new NotFoundException("Course not found.");
        
        return _mapper.Map<CourseDto>(course);
    }
}