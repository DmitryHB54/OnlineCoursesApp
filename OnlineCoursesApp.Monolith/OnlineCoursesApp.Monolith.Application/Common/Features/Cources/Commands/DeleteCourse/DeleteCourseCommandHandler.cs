using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.DeleteCourse;
public class DeleteCourseCommandHandler(
    ICourseRepository courseRepository,
    ICurrentUserService currentUserService,
    IFileStorageService fileStorageService,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken ct)
    {
        var course = await courseRepository.GetByIdForUpdateAsync(request.Id, ct)
            ?? throw new NotFoundException("Course not found");

        if (currentUserService.Role == RoleName.Teacher && course.AuthorId != currentUserService.UserId)
            throw new ForbiddenException("You can't delete someone else's course");

        await fileStorageService.DeleteCourseFolderAsync(course.Id, ct);
        courseRepository.Remove(course);
        await unitOfWork.SaveChangesAsync(ct);

        return Unit.Value;
    }
}