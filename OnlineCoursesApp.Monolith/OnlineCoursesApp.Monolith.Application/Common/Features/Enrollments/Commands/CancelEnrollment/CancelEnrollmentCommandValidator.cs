using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.CancelEnrollment;

public class CancelEnrollmentCommandValidator : AbstractValidator<CancelEnrollmentCommand>
{
    public CancelEnrollmentCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course id is required.");
    }
}