using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.CreateCourse;

public class CreateCourseCommandHandler
    : IRequestHandler<CreateCourseCommand, CourseDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService,
        IUnitOfWork unitOfWork,
        IMapper mapper) 
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CourseDto> Handle(
        CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = _mapper.Map<Course>(request);

        course.Id = Guid.NewGuid();
        course.AuthorId = _currentUserService.UserId;
        course.CreatedAt = DateTimeOffset.UtcNow;

        await _courseRepository.AddAsync(course, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CourseDto>(course);
    }
}