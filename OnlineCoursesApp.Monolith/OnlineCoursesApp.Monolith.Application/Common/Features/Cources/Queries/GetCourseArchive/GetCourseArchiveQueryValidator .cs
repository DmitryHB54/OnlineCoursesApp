using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseArchive;

public class GetCourseArchiveQueryValidator : AbstractValidator<GetCourseArchiveQuery>
{
    public GetCourseArchiveQueryValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course id required.");
    }
}