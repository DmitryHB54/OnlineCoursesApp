using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.CreateCourse;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.DeleteCourse;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Commands.UpdateCourse;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseArchive;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourseById;
using OnlineCoursesApp.Monolith.Application.Common.Features.Cources.Queries.GetCourses;
using OnlineCoursesApp.Monolith.Application.Models;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{
    private readonly IMediator _mediator;

    public CourseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CourseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetCoursesQuery request, CancellationToken cancellationToken)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCourseByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = RolesConstants.AdminOrTeacher)]
    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize(Roles = RolesConstants.AdminOrTeacher)]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        Guid id, [FromBody] UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(new { error = "Route id and body id must match." });
        }

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = RolesConstants.AdminOrTeacher)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCourseCommand(id), cancellationToken);
        return NoContent();
    }

    [Authorize(Roles = RolesConstants.AdminOrTeacher)]
    [HttpGet("{id:guid}/archive")]
    [ProducesResponseType(typeof(CourseArchiveFile), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetArchive(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCourseArchiveQuery(id), cancellationToken);
        return File(result.Content, result.ContentType, result.FileName);
    }
}