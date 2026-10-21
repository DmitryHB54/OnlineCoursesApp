using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.DownloadMaterial;
public class DownloadMaterialQueryValidator : AbstractValidator<DownloadMaterialQuery>
{
    public DownloadMaterialQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}