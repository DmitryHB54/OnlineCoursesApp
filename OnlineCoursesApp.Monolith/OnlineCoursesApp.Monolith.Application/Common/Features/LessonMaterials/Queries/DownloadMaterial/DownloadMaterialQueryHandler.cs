using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.DownloadMaterial;

public class DownloadMaterialQueryHandler
    : IRequestHandler<DownloadMaterialQuery, DownloadMaterial>
{
    private readonly ILessonMaterialRepository _lessonMaterialRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;

    public DownloadMaterialQueryHandler(
        ILessonMaterialRepository lessonMaterialRepository,
        IEnrollmentRepository enrollmentRepository,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUserService)
    {
        _lessonMaterialRepository = lessonMaterialRepository;
        _enrollmentRepository = enrollmentRepository;
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }

    public async Task<DownloadMaterial> Handle(DownloadMaterialQuery request, CancellationToken cancellationToken)
    {
        var material = await _lessonMaterialRepository.GetByIdWithCourseAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Material not found.");

        var canAccess = await HasAccessAsync(material, cancellationToken);
        if (!canAccess)
        {
            throw new ForbiddenException("There is no access to this material.");
        }

        var stream = await _fileStorageService.OpeanReadAsync(material.FileInfo.RelativePath, cancellationToken);
        var contentType = GetContentType(material.FileInfo.Extension);

        return new DownloadMaterial(stream, contentType, material.FileInfo.OriginalName);
    }

    private async Task<bool> HasAccessAsync(LessonMaterial material, CancellationToken cancellationToken)
    {
        switch(_currentUserService.Role)
        {
            case RoleName.Admin:
                return true;
            case RoleName.Teacher:
                return material.Course.AuthorId == _currentUserService.UserId;
            case RoleName.Student:
                return await _enrollmentRepository.ExistsAsync(
                    _currentUserService.UserId, material.CourseId, cancellationToken);
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