using Donnum.Gateway.Application.Features.Donations.Commands.CancelParticipation;
using Donnum.Gateway.Application.Features.Donations.Commands.CreateParticipation;
using Donnum.Gateway.Application.Models.Donor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DonationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{donorId:guid}/participation")]
    public async Task<IActionResult> CreateParticipation([FromRoute] Guid donorId, [FromBody] CreateParticipationDto request, CancellationToken cancellationToken)
    {
        if (donorId != request.DonorId)
        {
            return BadRequest("DonorId in route does not match DonorId in payload.");
        }

        var command = new CreateParticipationCommand(donorId, request);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result)
        {
            return Ok();
        }
        
        return BadRequest("Failed to create participation.");
    }

    [HttpPatch("{donorId:guid}/participation/{requestId:guid}")]
    public async Task<IActionResult> CancelParticipation([FromRoute] Guid donorId, [FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        var command = new CancelParticipationCommand(donorId, requestId);
        var result = await _mediator.Send(command, cancellationToken);
        
        if (result)
        {
            return NoContent();
        }

        return BadRequest("Failed to cancel participation.");
    }
}
