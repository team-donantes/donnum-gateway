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

    public async Task UpdateDonorAsync(Guid id, UpdateDonorDto request, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            Street = request.Street,
            City = request.City,
            Province = request.Province
        };
        var response = await _httpClient.PutAsJsonAsync($"/api/donors/{id}", payload, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<DonorDonationHistoryDto> GetDonationHistoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/donors/{id}/donations", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DonorDonationHistoryDto>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize donation history response.");
    }

    public async Task<IReadOnlyList<DonorBadgeDto>> GetDonorBadgesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/donors/{id}/badges", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<DonorBadgeDto>>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize badges response.");
    }

    public async Task<DonorReliabilityDto> GetDonorReliabilityAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/donors/{id}/reliability", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DonorReliabilityDto>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize reliability response.");
    }

    public async Task<bool> RegisterAttendanceAsync(Guid id, RegisterAttendanceRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/donors/{id}/attendance", request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<IReadOnlyList<DonorDto>> GetDonorsByRequestAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/donors/by-request/{requestId}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<DonorDto>>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize donors by request response.");
    }
}
