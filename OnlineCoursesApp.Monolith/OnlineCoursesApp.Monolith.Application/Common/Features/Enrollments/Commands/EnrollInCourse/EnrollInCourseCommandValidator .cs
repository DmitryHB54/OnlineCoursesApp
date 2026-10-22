using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.EnrollInCourse;
public class EnrollInCourseCommandValidator : AbstractValidator<EnrollInCourseCommand>
{
    public EnrollInCourseCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course id is required.");
    }
}