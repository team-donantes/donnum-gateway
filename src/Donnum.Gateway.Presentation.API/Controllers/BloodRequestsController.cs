using Donnum.Gateway.Application.Features.BloodRequests.Commands.CloseBloodRequest;
using Donnum.Gateway.Application.Features.BloodRequests.Commands.CreateBloodRequest;
using Donnum.Gateway.Application.Features.BloodRequests.Queries.GetActiveUrgencies;
using Donnum.Gateway.Application.Features.BloodRequests.Queries.GetBloodRequestById;
using Donnum.Gateway.Application.Features.BloodRequests.Queries.ListBloodRequests;
using Donnum.Gateway.Application.Models.BloodRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/blood-requests")]
public class BloodRequestsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? status,
        [FromQuery] string? type,
        [FromQuery] Guid? destinationMedicalCenterId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new ListBloodRequestsQuery(status, type, destinationMedicalCenterId, page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("active-urgencies")]
    public async Task<IActionResult> GetActiveUrgencies(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetActiveUrgenciesQuery(page, pageSize), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetBloodRequestByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBloodRequestBody body,
        CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirst("credential_id")?.Value;
        Guid operatorId;
        if (!Guid.TryParse(userIdString, out operatorId))
        {
            return Unauthorized("Identidad del operador no válida o token corrupto.");
        }

        var result = await mediator.Send(new CreateBloodRequestCommand(operatorId, body.DestinationMedicalCenterId, body.Title, body.Description, body.Type, body.RadiusKm, body.StartsAt, body.EndsAt, body.BloodTypes, body.Locations), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new CloseBloodRequestCommand(id), cancellationToken);
        return NoContent();
    }
}

public record CreateBloodRequestBody(
    Guid? DestinationMedicalCenterId,
    string Title,
    string? Description,
    string Type,
    float? RadiusKm,
    DateTime? StartsAt,
    DateTime? EndsAt,
    List<CreateBloodTypeDto> BloodTypes,
    List<CreateLocationDto>? Locations
);
