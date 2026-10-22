using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseArchive;

public record GetCourseArchiveQuery(Guid CourseId) : IRequest<CourseArchiveFile>;
public record CourseArchiveFile(Stream Content, string FileName, string ContentType);