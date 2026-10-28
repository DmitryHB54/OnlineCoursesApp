using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.CancelEnrollment;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.EnrollInCourse;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Queries.GetMyCourses;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api")]
public class EnrollmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EnrollmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = RolesConstants.Student)]
    [HttpPost("courses/{courseId:guid}/enrollment")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Enroll(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new EnrollInCourseCommand(courseId), cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = RolesConstants.Student)]
    [HttpDelete("courses/{courseId:guid}/enrollment")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Cancel(Guid courseId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelEnrollmentCommand(courseId), cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = RolesConstants.Student)]
    [HttpGet("me/courses")]
    [ProducesResponseType(typeof(List<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> MyCourses(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetMyCoursesQuery(), cancellationToken);
        return Ok(result);
    }
}