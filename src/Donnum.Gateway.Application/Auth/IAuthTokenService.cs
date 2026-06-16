namespace Donnum.Gateway.Application.Auth;

public interface IAuthTokenService
{
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
}
