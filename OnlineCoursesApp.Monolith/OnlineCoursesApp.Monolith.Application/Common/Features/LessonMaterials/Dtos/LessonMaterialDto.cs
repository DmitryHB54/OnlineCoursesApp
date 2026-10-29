using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;

public record LessonMaterialDto(
    Guid Id,
    Guid CourseId,
    string Title,
    uint OrderNumber,
    MaterialType MaterialType,
    DateTimeOffset CreatedAt,
    string OriginalFileName,
    long SizeBytes,
    string Extension);