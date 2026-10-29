using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesApp.Monolith.Application.AppConstants;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Commands.DeleteMaterial;
using OnlineCoursesApp.Monolith.Application.Common.Features.LessonMaterials.Queries.DownloadMaterial;

namespace OnlineCoursesApp.Monolith.WebApi.Controllers;

[ApiController]
[Route("api/materials")]
public class MaterialsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaterialsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DownloadMaterialQuery(id), cancellationToken);
        return File(result.Content, result.ContentType, result.FuleName);
    }

    [Authorize(Roles = RolesConstants.AdminOrTeacher)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteMaterialCommand(id), cancellationToken);
        return NoContent();
    }
}