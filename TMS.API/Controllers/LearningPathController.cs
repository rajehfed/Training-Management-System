using MediatR;
using Microsoft.AspNetCore.Mvc;

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


}
