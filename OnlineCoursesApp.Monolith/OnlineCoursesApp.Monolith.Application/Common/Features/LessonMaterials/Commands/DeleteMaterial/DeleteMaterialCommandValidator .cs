using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.DeleteMaterial;

public class DeleteMaterialCommandValidator : AbstractValidator<DeleteMaterialCommand>
{
    public DeleteMaterialCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}