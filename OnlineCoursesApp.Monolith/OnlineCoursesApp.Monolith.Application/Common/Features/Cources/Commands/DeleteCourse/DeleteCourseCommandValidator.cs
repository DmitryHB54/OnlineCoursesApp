using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.DeleteCourse;

public class DeleteCourseCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    public DeleteCourseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Course id required.");
    }
}