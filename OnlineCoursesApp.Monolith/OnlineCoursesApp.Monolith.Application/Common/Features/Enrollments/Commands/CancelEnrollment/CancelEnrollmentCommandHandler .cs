using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.CancelEnrollment;

public class CancelEnrollmentCommandHandler(
    IEnrollmentRepository enrollmentRepository,
    IUnitOfWork uniOfWork,
    ICurrentUserService currentUserService) : IRequestHandler<CancelEnrollmentCommand, Unit>
{
    public async Task<Unit> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
    {
        if(currentUserService.Role != RoleName.Student)
            throw new ForbiddenException("Only the student can unsubscribe from the course.");

        Enrollment enrollment = await enrollmentRepository
            .GetAsync(currentUserService.UserId, request.CourseId, cancellationToken)
                ?? throw new NotFoundException("Student isn't enrolled in this course.");

        enrollmentRepository.Remove(enrollment);
        await uniOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}