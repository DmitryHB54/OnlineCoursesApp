using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;
using OnlineCoursesApp.Monolith.Domain.Repositories;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.DeleteMaterial;

public class DeleteMaterialCommandHandler : IRequestHandler<DeleteMaterialCommand, Unit>
{
    private readonly ILessonMaterialRepository _lessonMaterialRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICurrentUserService _currentUserService;

    public DeleteMaterialCommandHandler(
        ILessonMaterialRepository lessonMaterialRepository,
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ICurrentUserService currentUserService)
    {
        _lessonMaterialRepository = lessonMaterialRepository;
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        var material = await _lessonMaterialRepository.GetByIdWithCourseAsync(request.Id, cancellationToken)
           ?? throw new NotFoundException("Material not found.");

        if (_currentUserService is { Role: RoleName.Teacher } && material.Course.AuthorId != request.Id) 
        {
            throw new ForbiddenException("It's prohibited to delete someone else's course content.");
        }

        await _fileStorageService.DeleteAsync(material.FileInfo.RelativePath, cancellationToken);

        _lessonMaterialRepository.Remove(material);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}