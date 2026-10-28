using FluentValidation;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;

public class UploadMaterialCommandValidator : AbstractValidator<UploadMaterialCommand>
{
    private static readonly string[] Allowed = { ".pdf", ".jpg", ".jpeg", ".webp", ".png", ".doc", ".docx" };
    private const long MaxSizeBytes = 20 * 1024 * 1024;

    public UploadMaterialCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(300);
        RuleFor(x => x.MaterialType).IsInEnum();

        RuleFor(x => x.FileName)
            .NotEmpty()
            .Must(name => Allowed.Contains(Path.GetExtension(name).ToLowerInvariant()))
            .WithMessage($"Only files with next extenstions are allowed: {string.Join(", ", Allowed)}");

        RuleFor(x => x.FileLength)
            .GreaterThan(0).WithMessage("File is empty.")
            .LessThanOrEqualTo(MaxSizeBytes).WithMessage("File more than 20 MB.");

        RuleFor(x => x.FileContent).NotNull();
    }
}