using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseArchive;

public class GetCourseArchiveQueryHandler(
    ICourseRepository courseRepository,
    ILessonMaterialRepository lessonMaterialRepository,
    IFileStorageService fileStorageService,
    ICurrentUserService currentUserService) : IRequestHandler<GetCourseArchiveQuery, CourseArchiveFile>
{
    public async Task<CourseArchiveFile> Handle(GetCourseArchiveQuery request, CancellationToken cancellationToken)
    {
        var course = await courseRepository.GetByIdAsync(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Course not found");

        if (currentUserService.Role == RoleName.Student)
            throw new ForbiddenException("The archive is only available to Admin and Teacher.");

        if (currentUserService.Role == RoleName.Teacher
            && course.AuthorId != currentUserService.UserId)
            throw new ForbiddenException("It's prohibited to download the archive of someone else's course.");

        var materials = await lessonMaterialRepository.GetByCourseIdAsync(course.Id, cancellationToken);
        if (materials.Count == 0)
            throw new NotFoundException("There is no materials in the course");

        var paths = materials.Select(m => m.FileInfo.RelativePath);
        var stream = await fileStorageService.CreateCourseArchiveAsync(course.Id, paths, cancellationToken);

        var safeFileName = $"course-{course.Id:N}-{DateTimeOffset.UtcNow:yyyyMMdd}.zip";
        return new CourseArchiveFile(stream, safeFileName, "application/zip");
    }
}