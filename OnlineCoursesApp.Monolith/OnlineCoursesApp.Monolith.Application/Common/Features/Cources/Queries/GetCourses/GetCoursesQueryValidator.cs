using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;

public class GetCoursesQueryValidator : AbstractValidator<GetCoursesQuery>
{
    public GetCoursesQueryValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}