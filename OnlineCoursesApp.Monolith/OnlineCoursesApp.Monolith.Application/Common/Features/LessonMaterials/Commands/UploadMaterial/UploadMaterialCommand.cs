using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;
public record UploadMaterialCommand(
    Guid CourseId,
    string Title,
    MaterialType MaterialType,
    string FileName,
    long FileLength,
    Stream FileContent) : IRequest<LessonMaterialDto>;