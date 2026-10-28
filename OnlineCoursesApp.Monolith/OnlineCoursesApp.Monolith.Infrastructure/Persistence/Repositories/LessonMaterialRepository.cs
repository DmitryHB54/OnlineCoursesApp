using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class LessonMaterialRepository : ILessonMaterialRepository
{
    private readonly AppDbContext _dbContext;

    public LessonMaterialRepository(
        AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<LessonMaterial?> GetByIdWithCourseAsync(Guid id, CancellationToken cancellationToken)
    {
        return _dbContext.LessonMaterials
            .Include(m => m.Course)
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<List<LessonMaterial>> GetByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
    {   
        return _dbContext.LessonMaterials
            .AsNoTracking()
            .Where(m => m.CourseId == courseId)
            .OrderBy(m => m.OrderNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task<uint> GetNextOrderNumberAsync(Guid courseId, CancellationToken cancellationToken)
    {
        var max = await _dbContext.LessonMaterials
            .Where(m => m.CourseId == courseId)
            .MaxAsync(m => (uint?)m.OrderNumber, cancellationToken);

        return (max ?? 0) + 1;
    }

    public async Task AddAsync(LessonMaterial material, CancellationToken cancellationToken)
    {
        await _dbContext.LessonMaterials.AddAsync(material, cancellationToken);
    }

    public void Remove(LessonMaterial material)
    {
        _dbContext.LessonMaterials.Remove(material);
    }
}