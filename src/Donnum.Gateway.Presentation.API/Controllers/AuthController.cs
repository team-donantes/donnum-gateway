using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Donnum.Gateway.Presentation.API.Controllers;

[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly IDonorServiceClient _donorServiceClient;

    public AuthController(IUserServiceClient userServiceClient, IDonorServiceClient donorServiceClient)
    {
        _userServiceClient = userServiceClient;
        _donorServiceClient = donorServiceClient;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(GatewayLoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var userResponse = await _userServiceClient.LoginAsync(request, cancellationToken);
            Guid? donorId = null;

            if (userResponse.Role == "Donor")
            {
                donorId = await _donorServiceClient.GetDonorIdByUserIdAsync(userResponse.Id, cancellationToken);
            }

            var response = new GatewayLoginResponseDto
            {
                AccessToken = userResponse.AccessToken,
                Role = userResponse.Role,
                Id = userResponse.Id,
                FirstName = userResponse.FirstName,
                LastName = userResponse.LastName,
                PhoneNumber = userResponse.PhoneNumber,
                ExpiresAt = userResponse.ExpiresAt,
                DonorId = donorId
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }
}
