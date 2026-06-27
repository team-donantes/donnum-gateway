using System.Net.Http.Headers;
using System.Net.Http.Json;
using Donnum.Gateway.Application.Auth;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Profile;
using Donnum.Gateway.Application.Models.User;
using Donnum.Gateway.Application.Models.Auth;
using Donnum.Gateway.Domain.Exceptions;

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

    public async Task<CreateOperatorIdentityResponseDto> CreateOperatorIdentityAsync(CreateOperatorIdentityRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/users/operators", request, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new DomainException($"Failed to create operator in User Service: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<CreateOperatorIdentityResponseDto>(cancellationToken: cancellationToken);
        return result ?? throw new DomainException("Failed to parse response from User Service");
    }

    public async Task<UserServiceLoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", request, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new DomainException($"Failed to login: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<UserServiceLoginResponseDto>(cancellationToken: cancellationToken);
        return result ?? throw new DomainException("Failed to parse response from User Service");
    }
}
