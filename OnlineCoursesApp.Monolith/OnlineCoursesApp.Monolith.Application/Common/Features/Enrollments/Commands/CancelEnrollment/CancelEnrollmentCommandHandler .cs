using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.CancelEnrollment;

public class CancelEnrollmentCommandHandler : IRequestHandler<CancelEnrollmentCommand, Unit>
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public CancelEnrollmentCommandHandler(
        IEnrollmentRepository enrollmentRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _enrollmentRepository = enrollmentRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(CancelEnrollmentCommand request, CancellationToken cancellationToken)
    {
        if(_currentUserService.Role != RoleName.Student)
        { 
            throw new ForbiddenException("Only the student can unsubscribe from the course.");
        }

        var enrollment = await _enrollmentRepository
            .GetAsync(_currentUserService.UserId, request.CourseId, cancellationToken)
                ?? throw new NotFoundException("Student isn't enrolled in this course.");

        _enrollmentRepository.Remove(enrollment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}