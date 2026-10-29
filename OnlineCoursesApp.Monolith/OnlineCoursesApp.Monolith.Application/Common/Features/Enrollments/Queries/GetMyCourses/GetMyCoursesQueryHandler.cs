using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Queries.GetMyCourses;

public class GetMyCoursesQueryHandler
    : IRequestHandler<GetMyCoursesQuery, List<CourseDto>>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetMyCoursesQueryHandler(
        IEnrollmentRepository enrollmentRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _enrollmentRepository = enrollmentRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<List<CourseDto>> Handle(GetMyCoursesQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService is not { Role: RoleName.Student })
        {
            throw new ForbiddenException("The list of your courses is available only to students.");
        }

        var cources = await _enrollmentRepository.GetUserCoursesAsync(_currentUserService.UserId, cancellationToken);

        return _mapper.Map<List<CourseDto>>(cources);
    }
}