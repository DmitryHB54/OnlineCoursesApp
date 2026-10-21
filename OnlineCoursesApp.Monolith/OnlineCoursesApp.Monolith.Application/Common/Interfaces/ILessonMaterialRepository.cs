using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;
public interface ILessonMaterialRepository
{
    Task<LessonMaterial?> GetByIdWithCourseAsync(Guid id, CancellationToken ct);
    Task<List<LessonMaterial>> GetByCourseIdAsync(Guid courseId, CancellationToken ct);
    Task<uint> GetNextOrderNumberAsync(Guid courseId, CancellationToken ct);
    Task AddAsync(LessonMaterial material, CancellationToken ct);
    void Remove(LessonMaterial material);
}