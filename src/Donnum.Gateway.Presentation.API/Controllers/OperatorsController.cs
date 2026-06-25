using Donnum.Gateway.Application.Features.Operators.Commands.AssignMedicalCenter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/operators")]
public class OperatorsController(IMediator mediator) : ControllerBase
{
    [HttpPut("{operatorId:guid}/medical-center")]
    public async Task<IActionResult> AssignMedicalCenter(
        Guid operatorId,
        [FromBody] AssignMedicalCenterBody body,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new AssignOperatorMedicalCenterCommand(operatorId, body.MedicalCenterId), cancellationToken);
        return NoContent();
    }
}

public record AssignMedicalCenterBody(Guid MedicalCenterId);
