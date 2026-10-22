using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.EnrollInCourse;

public class EnrollInCourseCommandHandler(
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
    IEnrollmentRepository enrollmentRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EnrollInCourseCommand, EnrollmentDto>
{
    public async Task<EnrollmentDto> Handle(EnrollInCourseCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.Role != RoleName.Student)
            throw new ForbiddenException("Only student can enroll in course.");

        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Course not found.");

        var exists = await enrollmentRepository.ExistsAsync(currentUserService.UserId,
            request.CourseId, cancellationToken);
        if (exists) 
            throw new ForbiddenException("Student is already enrolled in the course.");

        var enrollement = new Enrollment
        {
            UserId = currentUserService.UserId,
            CourseId = course.Id,
            EnrolledAt = DateTime.UtcNow
        };

        await enrollmentRepository.AddAsync(enrollement, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new EnrollmentDto(course.Id, course.Title, enrollement.EnrolledAt);
    }
}