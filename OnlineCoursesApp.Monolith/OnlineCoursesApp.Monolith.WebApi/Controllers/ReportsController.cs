using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController(
    IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet("courses")]
    public async Task<IActionResult> CoursesReport([FromQuery] DateTime from,
        [FromQuery] DateTime to,
        CancellationToken ct)
    {
        var result = await mediator.Send(new GetCoursesReportQuery(from, to), ct);
        return File(result.Content, result.ContentType, result.FileName);
    }
}