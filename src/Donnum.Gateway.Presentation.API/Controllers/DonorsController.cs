using Donnum.Gateway.Application.Features.Donors.Commands.CreateDonor;
using Donnum.Gateway.Application.Features.Donors.Commands.DeleteDonor;
using Donnum.Gateway.Application.Features.Donors.Commands.UpdateDonor;
using Donnum.Gateway.Application.Features.Donors.Queries.GetDonor;
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
    public async Task<IActionResult> CreateDonor([FromBody] CreateDonorCommand command)
    {
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
    public async Task<IActionResult> UpdateDonor(Guid id, [FromBody] UpdateDonorCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Id in the route does not match the Id in the request body.");
        }

        var result = await _mediator.Send(command);
        return Ok(result);
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
}
