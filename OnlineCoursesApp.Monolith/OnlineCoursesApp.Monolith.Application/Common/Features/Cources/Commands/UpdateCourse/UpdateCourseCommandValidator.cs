using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.UpdateCourse;
public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator() 
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(5000);
        RuleFor(x => x.Category).IsInEnum();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}