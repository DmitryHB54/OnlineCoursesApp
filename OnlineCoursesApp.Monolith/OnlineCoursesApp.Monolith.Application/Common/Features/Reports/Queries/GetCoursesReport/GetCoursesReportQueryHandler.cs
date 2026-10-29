using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Interfaces;
using OnlineCoursesApp.Monolith.Domain.Enums;
using System.Globalization;
using System.Text;

namespace OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;

public class GetCoursesReportQueryHandler
    : IRequestHandler<GetCoursesReportQuery, CoursesReportFile>
{
    private readonly IReportRepository _reportRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetCoursesReportQueryHandler(
        IReportRepository reportRepository,
        ICurrentUserService currentUserService)
    {
        _reportRepository = reportRepository;
        _currentUserService = currentUserService;
    }

    public async Task<CoursesReportFile> Handle(GetCoursesReportQuery request, CancellationToken cancellationToken)
    {
        Guid? authorFilter = _currentUserService.Role == RoleName.Teacher
            ? _currentUserService.UserId : null;

        var rows = await _reportRepository.GetCourseStatsAsync(
             request.From.ToUniversalTime(),
             request.To.ToUniversalTime(),
             authorFilter,
             cancellationToken);

        var bytes = BuildCsv(rows);
        var fileName = $"courses-report-{DateTimeOffset.UtcNow:yyyyMMdd-HHmmss-fff}.csv";

        return new CoursesReportFile(bytes, fileName, FilesConstants.CsvMimeType);
    }

    private static byte[] BuildCsv(List<CourseReportRowDto> rows)
    {
        using var stringWriter = new StringWriter();
        using (var csv = new CsvWriter(stringWriter, new CsvConfiguration(CultureInfo.InvariantCulture) {  Delimiter = ";" }))
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

        var csvString = stringWriter.ToString();
        var contentBytes = Encoding.UTF8.GetBytes(csvString);
        var bomBytes = Encoding.UTF8.GetPreamble();

        var result = new byte[bomBytes.Length + contentBytes.Length];
        Buffer.BlockCopy(bomBytes, 0, result, 0, bomBytes.Length);
        Buffer.BlockCopy(contentBytes, 0, result, bomBytes.Length, contentBytes.Length);

        return result;
    }

    private static string SafeCell(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;

        return value[0] is '=' or '+' or '-' or '@' ? "'" + value : value;
    }
}