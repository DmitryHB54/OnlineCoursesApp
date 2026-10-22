using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.DeleteMaterial;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.UploadMaterial;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Dtos;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.DownloadMaterial;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.GetCourseMaterials;
using OnlineCoursesApp.Monolith.Domain.Enums;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api/courses/{courseId:guid}/materials")]
public class CourseMaterialsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<LessonMaterialDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(Guid courseId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetCourseMaterialsQuery(courseId), ct);
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
        if(file is null || file.Length == 0)
            return BadRequest(new { error = "File is required." });

        await using var stream = file.OpenReadStream();

        var command = new UploadMaterialCommand(
            courseId, title, materialType, file.FileName, file.Length, stream);

        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

[ApiController]
[Route("api/materials")]
public class MaterialsController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("{id:guid}/download")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DownloadMaterialQuery(id), cancellationToken);
        return File(result.Content, result.ContentType, result.FuleName);
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteMaterialCommand(id), cancellationToken);
        return NoContent();
    }
}