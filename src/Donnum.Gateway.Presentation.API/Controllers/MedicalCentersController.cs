using Donnum.Gateway.Application.Features.MedicalCenters.Queries.GetMedicalCenters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/medical-centers")]
public class MedicalCentersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetMedicalCentersQuery(), cancellationToken);
        return Ok(result);
    }
}
