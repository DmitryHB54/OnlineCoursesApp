using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;

public record GetCourseMaterialsQuery(
    Guid CourseId,
    int Page = 1,
    int PageSize = 10) : IRequest<PagedResult<LessonMaterialDto>>;