using MediatR;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;
using OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;

public class GetCoursesReportQueryHandler(
    IReportRepository reportRepository,
    ICurrentUserService currentUserService) : IRequestHandler<GetCoursesReportQuery, CoursesReportFile>
{
    public async Task<CoursesReportFile> Handle(GetCoursesReportQuery request, CancellationToken cancellationToken)
    {
        Guid? authorFilter = currentUserService.Role == RoleName.Teacher
            ? currentUserService.UserId : null;

        var rows = await reportRepository.GetCourseStatsAsync(
             request.From.ToUniversalTime(),
             request.To.ToUniversalTime(),
             authorFilter,
             cancellationToken);

        var bytes = BuildCsv(rows);

        var bom = Encoding.UTF8.GetPreamble();
        var payload = new byte[bom.Length + bytes.Length];
        Buffer.BlockCopy(bom, 0, payload, 0, bom.Length);
        Buffer.BlockCopy(bytes, 0, payload, bom.Length, bytes.Length);

        var fileName = $"courses-report-{DateTime.UtcNow:yyyyMMdd-HHmmss}.csv";
        return new CoursesReportFile(payload, fileName, "text/csv; charset=utf-8");
    }

    private static byte[] BuildCsv(List<CourseReportRowDto> rows)
    {
        using var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, new UTF8Encoding(false), leaveOpen: true))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) {  Delimiter = "," }))
        {
            csv.WriteField("Title");
            csv.WriteField("AuthorEmail");
            csv.WriteField("Category");
            csv.WriteField("Price");
            csv.WriteField("MaterialsCount");
            csv.WriteField("EnrollmentsCount");
            csv.NextRecord();

            foreach (var row in rows)
            {
                csv.WriteField(SafeCell(row.Title));
                csv.WriteField(SafeCell(row.AuthorEmail));
                csv.WriteField(row.Category);
                csv.WriteField(row.Price);
                csv.WriteField(row.MaterialsCount);
                csv.WriteField(row.EnrollmentsCount);
                csv.NextRecord();
            }
        }

        return stream.ToArray();
    }

    private static string SafeCell(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value[0] is '=' or '+' or '-' or '@' ? "'" + value : value;
    }
}