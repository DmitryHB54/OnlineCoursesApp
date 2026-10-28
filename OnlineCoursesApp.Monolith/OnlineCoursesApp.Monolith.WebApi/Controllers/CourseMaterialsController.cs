using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;
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
    [ProducesResponseType(typeof(List<LessonMaterialDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(Guid courseId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCourseMaterialsQuery(courseId), cancellationToken);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [RequestSizeLimit(20 * 1024 * 1024)]
    [HttpPost]
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