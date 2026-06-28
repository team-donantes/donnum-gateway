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
        var token = Request.Headers.Authorization.ToString();
        var command = new CreateOperatorCommand(
            token,
            request.FirstName,
            request.LastName,
            request.Password,
            request.HospitalId,
            request.PersonalEmail
        );
        
        var result = await mediator.Send(command, cancellationToken);
        return Ok(new { OperatorId = result.OperatorId, CorporateEmail = result.CorporateEmail });
    }
}
