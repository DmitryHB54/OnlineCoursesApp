using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.CancelEnrollment;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Commands.EnrollInCourse;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.Enrollments.Queries.GetMyCourses;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api")]
public class EnrollmentsController(IMediator mediator) : ControllerBase
{
    [Authorize(Roles = "Student")]
    [HttpPost("courses/{courseId:guid}/enroll")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Enroll(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new EnrollInCourseCommand(courseId), cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = "Student")]
    [HttpDelete("courses/{courseId:guid}/enroll")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Cancel(Guid courseId, CancellationToken ct)
    {
        await mediator.Send(new CancelEnrollmentCommand(courseId), ct);
        return NoContent();
    }

    [Authorize(Roles = "Student")]
    [HttpGet("me/courses")]
    [ProducesResponseType(typeof(List<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> MyCourses(CancellationToken ct)
    {
        var result = await mediator.Send(new GetMyCoursesQuery(), ct);
        return Ok(result);
    }
}