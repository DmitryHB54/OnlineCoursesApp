using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.CreateCourse;
public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}