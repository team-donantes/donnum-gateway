using Donnum.Gateway.Application.Features.Operators.Commands.CreateOperator;
using Donnum.Gateway.Application.Models.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpPost("create-operator")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateOperator(
        [FromBody] CreateOperatorRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOperatorCommand(
            request.FirstName,
            request.LastName,
            request.Password,
            request.HospitalId,
            request.PersonalEmail
        );
        
        var operatorId = await mediator.Send(command, cancellationToken);
        return Ok(new { OperatorId = operatorId });
    }
}
