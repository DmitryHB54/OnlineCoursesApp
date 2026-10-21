using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;
public class GetCourseMaterialsQueryHandler(
    ILessonMaterialRepository lessonMaterialRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<GetCourseMaterialsQuery, List<LessonMaterialDto>>
{
    public async Task<List<LessonMaterialDto>> Handle(GetCourseMaterialsQuery request, CancellationToken cancellationToken)
    {
        var list = await lessonMaterialRepository.GetByCourseIdAsync(request.CourseId, cancellationToken);
        return list.Select(m => new LessonMaterialDto(m.Id, m.CourseId, m.Title, m.OrderNumber, m.MaterialType,
                    m.CreatedAt, m.FileInfo.OriginalName, m.FileInfo.SizeBytes, m.FileInfo.Extension)).ToList();
    }
}