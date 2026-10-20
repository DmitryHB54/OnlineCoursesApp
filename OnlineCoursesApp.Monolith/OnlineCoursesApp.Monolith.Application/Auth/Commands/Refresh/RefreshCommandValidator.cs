using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Auth.Commands.Refresh;
public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}