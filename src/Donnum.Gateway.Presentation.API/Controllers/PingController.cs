using Donnum.Gateway.Application.Features.Ping.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly IMediator _mediator;

    public PingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Ping([FromBody] PingCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new { Message = result });
    }
}
