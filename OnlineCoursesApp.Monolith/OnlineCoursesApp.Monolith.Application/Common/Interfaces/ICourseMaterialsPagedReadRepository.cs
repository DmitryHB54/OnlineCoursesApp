using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface ICourseMaterialsPagedReadRepository
{
    Task<PagedResult<LessonMaterialDto>> GetByCourseIdAsync(
        Guid courseId, int page, int pageSize, CancellationToken cancellationToken);
}