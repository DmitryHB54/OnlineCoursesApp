using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class CourseMaterialsPagedReadRepository : ICourseMaterialsPagedReadRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    public CourseMaterialsPagedReadRepository(
        AppDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedResult<LessonMaterialDto>> GetByCourseIdAsync(
        Guid courseId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.LessonMaterials.AsNoTracking().AsQueryable()
            .Where(m => m.CourseId == courseId).OrderBy(m => m.OrderNumber);

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<LessonMaterialDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<LessonMaterialDto>(items, page, pageSize, total);
    }
}