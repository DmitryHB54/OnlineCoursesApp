using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Infrastructure.Persistence.Repositories;

public class CourseFilteredReadRepository : ICourseFilteredReadRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CourseFilteredReadRepository(
        AppDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PagedResult<CourseDto>> GetFilteredAsync(CourseFilter filter, CancellationToken cancellationToken)
    {
        var query = _dbContext.Cources.AsNoTracking().AsQueryable();

        if (filter.Category.HasValue)
        {
            query = query.Where(c => c.Category == filter.Category.Value);
        }

        if (filter.AuthorId.HasValue)
        {
            query = query.Where(c => c.AuthorId == filter.AuthorId.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var pattern = $"%{filter.Search.Trim()}%";
            query = query.Where(c =>
                EF.Functions.ILike(c.Title, pattern) ||
                EF.Functions.ILike(c.Description ?? string.Empty, pattern)
            );
        }

        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "title" => query.OrderBy(c => c.Title),
            "price" => query.OrderBy(c => c.Price),
            "createdat" => query.OrderByDescending(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<CourseDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<CourseDto>(items, filter.Page, filter.PageSize, total);
    }
}