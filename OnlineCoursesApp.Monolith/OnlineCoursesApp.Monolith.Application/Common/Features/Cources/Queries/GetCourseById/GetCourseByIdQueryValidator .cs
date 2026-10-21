using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseById;
public class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}