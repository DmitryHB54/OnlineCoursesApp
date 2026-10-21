using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.DeleteMaterial;
public record DeleteMaterialCommand(Guid Id) : IRequest<Unit>;