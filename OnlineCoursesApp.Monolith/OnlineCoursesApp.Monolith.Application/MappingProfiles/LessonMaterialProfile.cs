using AutoMapper;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Domain.Entities;

namespace OnlineCoursesApp.Monolith.Application.MappingProfiles;
public class LessonMaterialProfile : Profile
{
    public LessonMaterialProfile()
    {
        CreateMap<LessonMaterial, LessonMaterialDto>()
            .ConstructUsing(src => new LessonMaterialDto(
                src.Id,
                src.CourseId,
                src.Title,
                src.OrderNumber,
                src.MaterialType,
                src.CreatedAt,
                src.FileInfo.OriginalName,
                src.FileInfo.SizeBytes,
                src.FileInfo.Extension
            ));
    }
}