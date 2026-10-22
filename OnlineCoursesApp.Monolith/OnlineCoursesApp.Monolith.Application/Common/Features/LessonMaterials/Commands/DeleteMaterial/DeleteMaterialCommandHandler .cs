using MediatR;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Entities;
using OnlineCoursesApp.Monolith.Domain.Enums;
using OnlineCoursesApp.Monolith.Domain.Exceptions;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.DeleteMaterial;
public class DeleteMaterialCommandHandler(
    ILessonMaterialRepository lessonMaterialRepository,
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorageService,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteMaterialCommand, Unit>
{
    public async Task<Unit> Handle(DeleteMaterialCommand request, CancellationToken cancellationToken)
    {
        LessonMaterial material = await lessonMaterialRepository.GetByIdWithCourseAsync(request.Id, cancellationToken)
           ?? throw new NotFoundException("Material not found.");

        if (currentUserService.Role == RoleName.Teacher && material.Course.AuthorId != request.Id)
            throw new ForbiddenException("It's prohibited to delete someone else's course content.");

        await fileStorageService.DeleteAsync(material.FileInfo.RelativePath, cancellationToken);

        lessonMaterialRepository.Remove(material);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}