using MediatR;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseArchive;

public class GetCourseArchiveQueryHandler 
    : IRequestHandler<GetCourseArchiveQuery, CourseArchiveFile>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILessonMaterialRepository _lessonMaterialRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;

    public GetCourseArchiveQueryHandler(
        ICourseRepository courseRepository,
        ILessonMaterialRepository lessonMaterialRepository,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUserService)
    {
        _courseRepository = courseRepository;
        _lessonMaterialRepository = lessonMaterialRepository;
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }

    public async Task<CourseArchiveFile> Handle(GetCourseArchiveQuery request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Course not found");

        if (_currentUserService.Role == RoleName.Student)
        {
            throw new ForbiddenException("The archive is only available to Admin and Teacher.");
        }

        if (_currentUserService.Role == RoleName.Teacher
            && course.AuthorId != _currentUserService.UserId)
        {
            throw new ForbiddenException("It's prohibited to download the archive of someone else's course.");
        }

        var materials = await _lessonMaterialRepository.GetByCourseIdAsync(course.Id, cancellationToken);
        if (materials.Count == 0)
        { 
            throw new NotFoundException("There is no materials in the course");
        }

        var paths = materials.Select(m => m.FileInfo.RelativePath);
        var stream = await _fileStorageService.CreateCourseArchiveAsync(course.Id, paths, cancellationToken);

        var safeFileName = $"course-{course.Id:N}-{DateTimeOffset.UtcNow:yyyyMMdd}.zip";
        return new CourseArchiveFile(stream, safeFileName, FilesConstants.ZipMimeType);
    }
}