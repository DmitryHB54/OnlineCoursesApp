using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class LessonMaterialRepository(
    AppDbContext dbContext) : ILessonMaterialRepository
{
    public Task<LessonMaterial?> GetByIdWithCourseAsync(Guid id, CancellationToken ct)
        => dbContext.LessonMaterials
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    public Task<List<LessonMaterial>> GetByCourseIdAsync(Guid courseId, CancellationToken ct)
        => dbContext.LessonMaterials
            .AsNoTracking()
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.OrderNumber)
            .ToListAsync(ct);
    public async Task<uint> GetNextOrderNumberAsync(Guid courseId, CancellationToken ct)
    {
        var max = await dbContext.LessonMaterials
            .Where(m => m.CourseId == courseId)
            .MaxAsync(m => (uint?)m.OrderNumber, ct);
        return (max ?? 0) + 1;
    }
    public async Task AddAsync(LessonMaterial material, CancellationToken ct)
        => await dbContext.LessonMaterials.AddAsync(material, ct);
    public void Remove(LessonMaterial material)
        => dbContext.LessonMaterials.Remove(material);
}