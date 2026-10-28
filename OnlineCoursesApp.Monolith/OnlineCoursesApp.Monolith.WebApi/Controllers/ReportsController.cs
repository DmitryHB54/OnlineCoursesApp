using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.Common.Features.Reports.Queries.GetCoursesReport;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet("courses")]
    [ProducesResponseType(typeof(GetCoursesReportQuery), StatusCodes.Status200OK)]
    public async Task<IActionResult> CoursesReport([FromQuery] DateTime from,
        [FromQuery] DateTime to, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCoursesReportQuery(from, to), cancellationToken);
        return File(result.Content, result.ContentType, result.FileName);
    }
}