using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.DeleteCourse;

public class DeleteCourseCommandHandler 
    : IRequestHandler<DeleteCourseCommand, Unit>
{
    private readonly ICourseRepository _courseRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCourseCommandHandler(
        ICourseRepository courseRepository,
        ICurrentUserService currentUserService,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _currentUserService = currentUserService;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdForUpdateAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Course not found");

        if (_currentUserService.Role == RoleName.Teacher && course.AuthorId != _currentUserService.UserId)
        { 
            throw new ForbiddenException("You can't delete someone else's course");
        }

        await _fileStorageService.DeleteCourseFolderAsync(course.Id, cancellationToken);

        _courseRepository.Remove(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}