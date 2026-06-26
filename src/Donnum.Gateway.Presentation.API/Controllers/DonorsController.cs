using Donnum.Gateway.Application.Features.Donors.Commands.CreateDonor;
using Donnum.Gateway.Application.Features.Donors.Commands.DeleteDonor;
using Donnum.Gateway.Application.Features.Donors.Commands.UpdateDonor;
using Donnum.Gateway.Application.Features.Donors.Queries.GetDonor;
using Donnum.Gateway.Application.Features.Donors.Queries.GetDonationHistory;
using Donnum.Gateway.Application.Features.Donors.Queries.GetDonorBadges;
using Donnum.Gateway.Application.Features.Donors.Queries.GetDonorReliability;
using Donnum.Gateway.Application.Features.Donors.Commands.RegisterAttendance;
using Donnum.Gateway.Application.Features.Donors.Queries.GetDonorsByRequest;
using Donnum.Gateway.Application.Models.Donor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DonorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public async Task<IActionResult> CreateDonor([FromBody] CreateDonorCommand command)
    {
        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                        ?? User.FindFirst("sub")?.Value;

        if (Guid.TryParse(userIdStr, out var userId) && command.AuthUserId == Guid.Empty)
        {
            command = command with { AuthUserId = userId };
        }

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetDonor), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDonor(Guid id)
    {
        var query = new GetDonorQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDonor(Guid id, [FromBody] UpdateDonorDto dto)
    {
        var command = new UpdateDonorCommand(id, dto.Street, dto.City, dto.Province, dto.PhoneNumber);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDonor(Guid id)
    {
        var command = new DeleteDonorCommand(id);
        var result = await _mediator.Send(command);
        
        if (result)
        {
            return NoContent();
        }
        
        return NotFound();
    }

    [HttpGet("{id}/donations")]
    public async Task<IActionResult> GetDonationHistory(Guid id)
    {
        var query = new GetDonationHistoryQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/badges")]
    public async Task<IActionResult> GetDonorBadges(Guid id)
    {
        var query = new GetDonorBadgesQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/reliability")]
    public async Task<IActionResult> GetDonorReliability(Guid id)
    {
        var query = new GetDonorReliabilityQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{id}/attendance")]
    public async Task<IActionResult> RegisterAttendance(Guid id, [FromBody] RegisterAttendanceRequest payload)
    {
        var command = new RegisterAttendanceCommand(id, payload);
        var result = await _mediator.Send(command);
        
        if (result)
        {
            return NoContent();
        }
        
        return BadRequest();
    }

    [HttpGet("by-request/{requestId}")]
    public async Task<IActionResult> GetDonorsByRequest(Guid requestId)
    {
        var query = new GetDonorsByRequestQuery(requestId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
