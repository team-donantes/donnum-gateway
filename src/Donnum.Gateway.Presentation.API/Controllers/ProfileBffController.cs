using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Donnum.Gateway.Application.Features.Profiles.Queries.GetCombinedDonorProfile;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/donors/me")]
[Authorize]
public class ProfileBffController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCombinedProfile(CancellationToken cancellationToken)
    {
        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authHeader))
        {
            return Unauthorized();
        }

        var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                        ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdStr, out var userId))
        {
            return BadRequest("El token no contiene un identificador de usuario válido (sub).");
        }

        var query = new GetCombinedDonorProfileQuery(userId, authHeader);
        var result = await mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound("Identidad no encontrada.");
        }

        return Ok(result);
    }
}
