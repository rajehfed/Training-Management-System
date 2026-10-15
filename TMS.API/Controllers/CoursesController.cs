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

namespace TMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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

    [Authorize]
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

    [Authorize]
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

    [Authorize]
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

    [Authorize]
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

    [Authorize]
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
}