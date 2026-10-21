using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Learning.CourseApp.Commands.ArchiveCourse;
using TMS.Application.Learning.CourseApp.Commands.CreateCourse;
using TMS.Application.Learning.CourseApp.Commands.PublishCourse;
using TMS.Application.Learning.CourseApp.Commands.UpdateCourse;
using TMS.Application.Learning.CourseApp.Commands.UpdateCourseDescription;
using TMS.Application.Learning.CourseApp.Commands.UpdateCourseDuration;
using TMS.Application.Learning.CourseApp.Commands.UpdateCourseTitle;
using TMS.Application.Learning.CourseApp.Queries.GetAllCourses;
using TMS.Application.Learning.CourseApp.Queries.GetCourseById;
using TMS.Application.Learning.CourseApp.Queries.GetCourseByName;

namespace TMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseCommand command, CancellationToken cancellationToken)
    {
        if (id != command.CourseId)
        {
            return BadRequest("The course ID in the path does not match the ID in the request.");
        }

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("{id:int}/publish")]
    public async Task<IActionResult> PublishCourse(int id, CancellationToken cancellationToken)
    {
        var command = new PublishCourseCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("{id:int}/archive")]
    public async Task<IActionResult> ArchiveCourse(int id, CancellationToken cancellationToken)
    {
        var command = new ArchiveCourseCommand(id);
        var result = await _mediator.Send(command, cancellationToken);

        if(!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:int}/duration")]
    public async Task<IActionResult> UpdateCourseDuration(
        int id,
        [FromBody] UpdateCourseDurationCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CourseId)
        {
            return BadRequest("The course ID in the path does not match the ID in the request.");
        }
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("{id:int}/description")]
    public async Task<IActionResult> UpdateCourseDescription(
        int id,
        [FromBody] UpdateCourseDescriptionCommand command,
        CancellationToken cancellationToken)
    {
        if(id != command.CourseId)
            return BadRequest("The course ID in the path does not match the ID in the request.");

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpPut("{id:int}/title")]
    public async Task<IActionResult> UpdateCourseTitle(
        int id,
        [FromBody] UpdateCourseTitleCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.CourseId)
            return BadRequest("The course ID in the path does not match the ID in the request.");
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize("Admin")]
    public async Task<IActionResult> GetCourseById(int id, CancellationToken cancellationToken)
    {
        var query = new GetCourseByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }
    [HttpGet("title")]
    public async Task<IActionResult> GetCourseByName(string title, CancellationToken cancellationToken)
    {
        var query = new GetCourseByTitleQuery(title);
        var result = await _mediator.Send(query, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourses(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        CancellationToken cancellationToken = default)
    {
        var query = new GetAllCoursesQuery(pageNumber, pageSize, searchTerm);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}