using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;

public record GetCoursesReportQuery(DateTimeOffset From, DateTimeOffset To) : IRequest<CoursesReportFile>;

public record CoursesReportFile(byte[] Content, string FileName, string ContentType);