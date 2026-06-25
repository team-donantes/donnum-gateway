using System.Net.Http.Headers;
using System.Net.Http.Json;
using Donnum.Gateway.Application.Auth;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Profile;

namespace Donnum.Gateway.Infrastructure.HttpClients;

public class UserServiceClient(HttpClient httpClient) : IAuthTokenService, IUserServiceClient
{
    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(true); // Placeholder
    }

    public async Task<IdentityProfileDto?> GetMeAsync(string token, CancellationToken cancellationToken = default)
    {
        using var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/me");
        requestMessage.Headers.Authorization = AuthenticationHeaderValue.Parse(token);
        
        var response = await httpClient.SendAsync(requestMessage, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<IdentityProfileDto>(cancellationToken: cancellationToken);
    }
}
