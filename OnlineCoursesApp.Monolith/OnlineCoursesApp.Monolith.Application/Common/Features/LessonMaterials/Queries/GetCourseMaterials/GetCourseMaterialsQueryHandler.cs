using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;

public class GetCourseMaterialsQueryHandler
    : IRequestHandler<GetCourseMaterialsQuery, List<LessonMaterialDto>>
{
    private readonly ILessonMaterialRepository _lessonMaterialRepository;

    public GetCourseMaterialsQueryHandler(
        ILessonMaterialRepository lessonMaterialRepository)
    {
        _lessonMaterialRepository = lessonMaterialRepository;
    }

    public async Task<List<LessonMaterialDto>> Handle(GetCourseMaterialsQuery request, CancellationToken cancellationToken)
    {
        var list = await _lessonMaterialRepository.GetByCourseIdAsync(request.CourseId, cancellationToken);

        return list.Select(m => new LessonMaterialDto(m.Id, m.CourseId, m.Title, m.OrderNumber, m.MaterialType,
                    m.CreatedAt, m.FileInfo.OriginalName, m.FileInfo.SizeBytes, m.FileInfo.Extension)).ToList();
    }
}