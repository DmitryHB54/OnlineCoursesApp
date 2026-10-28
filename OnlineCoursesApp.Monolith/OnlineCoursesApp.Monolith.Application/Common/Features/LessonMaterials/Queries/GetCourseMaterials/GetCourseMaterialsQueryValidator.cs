using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;

public class GetCourseMaterialsQueryValidator : AbstractValidator<GetCourseMaterialsQuery>
{
    public GetCourseMaterialsQueryValidator() 
    {
        RuleFor(x => x.CourseId).NotEmpty().WithMessage("Course id is required.");
    }
}