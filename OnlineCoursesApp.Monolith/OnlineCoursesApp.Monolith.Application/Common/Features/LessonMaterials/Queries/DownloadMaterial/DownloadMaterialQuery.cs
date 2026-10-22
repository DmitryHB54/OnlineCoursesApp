using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.DownloadMaterial;

public record DownloadMaterialQuery(Guid Id) : IRequest<DownloadMaterial>;

public record DownloadMaterial(Stream Content, string ContentType, string FuleName);