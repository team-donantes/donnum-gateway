using Donnum.Gateway.Application.Auth;

namespace Donnum.Gateway.Infrastructure.HttpClients;

public class UserServiceClient : IAuthTokenService
{
    private readonly HttpClient _httpClient;

    public UserServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        // TODO: Implement actual call to User Service to validate the token
        // Example:
        // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        // var response = await _httpClient.GetAsync("/api/auth/validate", cancellationToken);
        // return response.IsSuccessStatusCode;

        return await Task.FromResult(true); // Placeholder
    }
}
