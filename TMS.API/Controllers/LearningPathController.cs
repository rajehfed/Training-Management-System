using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Learning.LearningPathApp.Commands.AddCourseToPath;
using TMS.Application.Learning.LearningPathApp.Commands.AddNewLearningPath;
using TMS.Application.Learning.LearningPathApp.Commands.ArchiveLearningPath;
using TMS.Application.Learning.LearningPathApp.Commands.PublishLearningPath;
using TMS.Application.Learning.LearningPathApp.Commands.RemoveCourseFromPath;
using TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPath;
using TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathAudience;
using TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathName;
using TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathOutcomes;
using TMS.Application.Learning.LearningPathApp.Queries.GetAllLearningPaths;
using TMS.Application.Learning.LearningPathApp.Queries.GetLearningPathById;
using TMS.Application.Learning.LearningPathApp.Queries.GetLearningPathByName;
using TMS.Domain.Shared.Results;

namespace TMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LearningPathController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearningPathController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateLearningPath(
        [FromBody] AddNewLearningPathCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("UpdateLearningPathDetails/{id:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateLearningPathDetails(
        int id,
        [FromBody] UpdateLearningPathCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.PathId)
        {
            return BadRequest("The learning path ID in the path parameter does not match the body payload.");
        }

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("{id:int}/courses/AddCourseToPath/{courseId:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddCourseToPath(
        int id,
        int courseId,
        int orderInPath,
        CancellationToken cancellationToken)
    {
        var command = new AddCourseToPathCommand(id, courseId, orderInPath);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("UpdateLearningPathName/{id:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePathName(
        int id,
        [FromBody] UpdateLearningPathNameCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.PathId)
        {
            return BadRequest("The learning path ID in the path parameter does not match the body payload.");
        }
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPatch("UpdateLearningPathAudience/{id:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePathAudience(
        int id,
        [FromBody] UpdateLearningPathAudienceCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.PathId)
        {
            return BadRequest("The learning path ID in the path parameter does not match the body payload.");
        }
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPatch("UpdateLearningPathOutcomes/{id:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePathOutcomes(
        int id,
        [FromBody] UpdateLearningPathOutcomesCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.PathId)
        {
            return BadRequest("The learning path ID in the path parameter does not match the body payload.");
        }

        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }

    [HttpPatch("RemoveCourseFromPath/{id:int}/{courseId:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveCourseFromPath(
        int id,
        int courseId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCourseFromPathCommand(id, courseId);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("PublishLearningPath/{id:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PublishLearningPath(int id, CancellationToken cancellationToken)
    {
        var command = new PublishLearningPathCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPatch("ArchiveLearningPath/{id:int}")]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ArchiveLearningPath(int id, CancellationToken cancellationToken)
    {
        var command = new ArchiveLearningPathCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("GetLearningPathById/{id:int}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLearningPathById(int id, CancellationToken cancellationToken)
    {
        var query = new GetLearningPathByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet("GetLearningPathByName")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResultBase), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetLearningPathByName(
        [FromQuery] string name, 
        CancellationToken cancellationToken)
    {
        var query = new GetLearningPathByNameQuery(name);
        var result = await _mediator.Send(query, cancellationToken);

        if (result is null)
            return NotFound(result);

        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLearningPaths(
        //[FromQuery] int pageNumber = 1,
        //[FromQuery] int pageSize = 10,
        //[FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllLearningPathsQuery();
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
