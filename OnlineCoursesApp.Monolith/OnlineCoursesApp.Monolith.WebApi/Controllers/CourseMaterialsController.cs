using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;
using OnlineCoursesApp.Monolith.Application.Models;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api/courses/{courseId:guid}/materials")]
public class CourseMaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CourseMaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<LessonMaterialDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(Guid courseId, CancellationToken cancellationToken, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetCourseMaterialsQuery(courseId, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = RolesConstants.AdminOrTeacher)]
    [HttpPost]
    [RequestSizeLimit(FilesConstants.MaxFileSizeBytes)]
    [ProducesResponseType(typeof(LessonMaterialDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(Guid courseId, [FromForm] string title,
        [FromForm] MaterialType materialType, IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { error = "File is required." });
        }

        await using var stream = file.OpenReadStream();
        var command = new UploadMaterialCommand(
            courseId, title, materialType, file.FileName, file.Length, stream);

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}