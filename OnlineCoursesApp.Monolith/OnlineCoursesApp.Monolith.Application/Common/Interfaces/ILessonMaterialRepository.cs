using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.Common.Interfaces;

public interface ILessonMaterialRepository
{
    Task<LessonMaterial?> GetByIdWithCourseAsync(Guid id, CancellationToken cancellationToken);

    Task<List<LessonMaterial>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);

    Task<uint> GetNextOrderNumberAsync(Guid courseId, CancellationToken cancellationToken);

    Task AddAsync(LessonMaterial material, CancellationToken cancellationToken);

    void Remove(LessonMaterial material);
}