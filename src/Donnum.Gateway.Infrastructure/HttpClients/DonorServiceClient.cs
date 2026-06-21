using System.Net.Http.Json;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;

namespace Donnum.Gateway.Infrastructure.HttpClients;

public class DonorServiceClient : IDonorServiceClient
{
    private readonly HttpClient _httpClient;

    public DonorServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DonorDto> CreateDonorAsync(CreateDonorDto request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/donors", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DonorDto>(cancellationToken: cancellationToken) 
               ?? throw new InvalidOperationException("Failed to deserialize donor response.");
    }

    public async Task<bool> DeleteDonorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync($"/api/donors/{id}", cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<DonorDto> GetDonorAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/donors/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DonorDto>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize donor response.");
    }

    public async Task<DonorDto> UpdateDonorAsync(Guid id, UpdateDonorDto request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/donors/{id}", request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DonorDto>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize donor response.");
    }
}
