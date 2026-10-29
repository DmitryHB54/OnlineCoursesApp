using AutoMapper;
using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;

public class GetCourseMaterialsQueryHandler
    : IRequestHandler<GetCourseMaterialsQuery, PagedResult<LessonMaterialDto>>
{
    private readonly ICourseMaterialsPagedReadRepository _lessonMaterialPagedReadRepository;
    private readonly ICourseRepository _courseRepository;

    public GetCourseMaterialsQueryHandler(
        ICourseMaterialsPagedReadRepository lessonMaterialPagedReadRepository,
        ICourseRepository courseRepository)
    {
        _lessonMaterialPagedReadRepository = lessonMaterialPagedReadRepository;
        _courseRepository = courseRepository;
    }

    public async Task<PagedResult<LessonMaterialDto>> Handle(GetCourseMaterialsQuery request, CancellationToken cancellationToken)
    {
        var exists = await _courseRepository.ExistsByIdAsync(request.CourseId, cancellationToken);
        if(!exists)
            throw new NotFoundException("Course not found.");

        var result = await _lessonMaterialPagedReadRepository
            .GetByCourseIdAsync(request.CourseId, request.Page, request.PageSize, cancellationToken);

        return result;
    }
}