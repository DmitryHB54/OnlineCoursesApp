using MediatR;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;
public record GetCoursesReportQuery(DateTime From, DateTime To) : IRequest<CoursesReportFile>;
public record CoursesReportFile(byte[] Content, string FileName, string ContentType);