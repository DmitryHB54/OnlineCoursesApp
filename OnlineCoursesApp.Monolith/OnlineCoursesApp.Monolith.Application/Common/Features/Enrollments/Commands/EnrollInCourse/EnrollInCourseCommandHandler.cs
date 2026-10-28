using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.EnrollInCourse;

public class EnrollInCourseCommandHandler
    : IRequestHandler<EnrollInCourseCommand, EnrollmentDto>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EnrollInCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService,
        IEnrollmentRepository enrollmentRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<EnrollmentDto> Handle(EnrollInCourseCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.Role != RoleName.Student)
        {
            throw new ForbiddenException("Only student can enroll in course.");
        }

        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Course not found.");

        var exists = await _enrollmentRepository.ExistsAsync(_currentUserService.UserId,
            request.CourseId, cancellationToken);

        if (exists)
        {
            throw new ForbiddenException("Student is already enrolled in the course.");
        }

        var enrollement = new Enrollment
        {
            UserId = _currentUserService.UserId,
            CourseId = course.Id,
            EnrolledAt = DateTime.UtcNow
        };

        await _enrollmentRepository.AddAsync(enrollement, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new EnrollmentDto(course.Id, course.Title, enrollement.EnrolledAt);
    }
}