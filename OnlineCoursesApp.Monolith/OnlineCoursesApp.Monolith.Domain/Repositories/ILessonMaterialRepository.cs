using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Domain.Repositories;

public interface ILessonMaterialRepository : IRepository<LessonMaterial>
{
    Task<LessonMaterial?> GetByIdWithCourseAsync(Guid id, CancellationToken cancellationToken);

    Task<List<LessonMaterial>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);

    Task<uint> GetNextOrderNumberAsync(Guid courseId, CancellationToken cancellationToken);
}