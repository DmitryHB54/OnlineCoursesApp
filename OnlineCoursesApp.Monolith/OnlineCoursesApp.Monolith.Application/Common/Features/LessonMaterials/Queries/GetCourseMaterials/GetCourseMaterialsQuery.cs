using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;

public record GetCourseMaterialsQuery(Guid CourseId) : IRequest<List<LessonMaterialDto>>;