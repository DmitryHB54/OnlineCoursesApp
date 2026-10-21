using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.DownloadMaterial;

public class DownloadMaterialQueryHandler(
    ILessonMaterialRepository lessonMaterialRepository,
    IEnrollmentRepository enrollmentRepository,
    IFileStorageService fileStorageService,
    ICurrentUserService currentUserService) : IRequestHandler<DownloadMaterialQuery, DownloadMaterial>
{
    public async Task<DownloadMaterial> Handle(DownloadMaterialQuery request, CancellationToken cancellationToken)
    {
        var material = await lessonMaterialRepository.GetByIdWithCourseAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Material not found.");

        var canAccess = await HasAccessAsync(material, cancellationToken);
        if (!canAccess)
            throw new ForbiddenException("There is no access to this material.");

        var stream = await fileStorageService.OpeanReadAsync(material.FileInfo.RelativePath, cancellationToken);
        var contentType = GetContentType(material.FileInfo.Extension);

        return new DownloadMaterial(stream, contentType, material.FileInfo.OriginalName);
    }

    private async Task<bool> HasAccessAsync(LessonMaterial material, CancellationToken cancellationToken)
    {
        switch(currentUserService.Role)
        {
            case RoleName.Admin:
                return true;
            case RoleName.Teacher:
                return material.Course.AuthorId == currentUserService.UserId;
            case RoleName.Student:
                return await enrollmentRepository.ExistsAsync(
                    currentUserService.UserId, material.CourseId, cancellationToken);
            default:
                return false;
        }
    }

    private static string GetContentType(string ext) => ext.ToLowerInvariant() switch
    {
        ".pdf" => "application/pdf",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".webp" => "image/webp",
        ".doc" => "application/msword",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        _ => "application/octet-stream"
    };
}