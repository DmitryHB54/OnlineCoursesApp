using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;

public class UploadMaterialCommandHandler(
    ILessonMaterialRepository lessonMaterialRepository,
    ICourseRepository courseRepository,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService,
    ICurrentUserService currentUserService) : IRequestHandler<UploadMaterialCommand, LessonMaterialDto>
{
    public async Task<LessonMaterialDto> Handle(UploadMaterialCommand request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Курс не найден.");

        if(currentUserService.Role == RoleName.Teacher && currentUserService.UserId != course.AuthorId)
            throw new ForbiddenException("It's prohibited to add content to someone else's course.");

        StoredFileResult stored = await fileStorageService.SaveAsync(
            request.CourseId, request.FileContent, request.FileName, cancellationToken);

        uint orderNumber = await lessonMaterialRepository.GetNextOrderNumberAsync(request.CourseId, cancellationToken);

        var material = new LessonMaterial
        {
            Id = Guid.NewGuid(),
            CourseId = request.CourseId,
            Title = request.Title.Trim(),
            OrderNumber = orderNumber,
            MaterialType = request.MaterialType,
            CreatedAt = DateTime.UtcNow,
            FileInfo = new Domain.Entities.FileInfo(
                stored.RelativePath,
                request.FileName,
                stored.SizeBytes,
                stored.Extension)
        };

        await lessonMaterialRepository.AddAsync(material, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LessonMaterialDto(
            material.Id, material.CourseId, material.Title,
            material.OrderNumber, material.MaterialType, material.CreatedAt,
            material.FileInfo.OriginalName, material.FileInfo.SizeBytes,
            material.FileInfo.Extension);
    }
}