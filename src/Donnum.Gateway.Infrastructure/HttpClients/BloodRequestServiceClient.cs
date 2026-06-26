using System.Net.Http.Json;
using System.Text;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;

namespace Donnum.Gateway.Infrastructure.HttpClients;

public class BloodRequestServiceClient : IBloodRequestServiceClient
{
    private readonly HttpClient _httpClient;

    public BloodRequestServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PagedBloodRequestResult> GetBloodRequestsAsync(string? status, string? type, Guid? destinationMedicalCenterId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new List<string> { $"page={page}", $"pageSize={pageSize}" };
        if (!string.IsNullOrEmpty(status)) query.Add($"status={status}");
        if (!string.IsNullOrEmpty(type)) query.Add($"type={type}");
        if (destinationMedicalCenterId.HasValue) query.Add($"destinationMedicalCenterId={destinationMedicalCenterId}");

        var response = await _httpClient.GetAsync($"/api/blood-requests?{string.Join("&", query)}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PagedBloodRequestResult>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize blood requests response.");
    }

    public async Task<PagedBloodRequestResult> GetActiveUrgenciesAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/blood-requests/active-urgencies?page={page}&pageSize={pageSize}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PagedBloodRequestResult>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize active urgencies response.");
    }

    public async Task<BloodRequestDto> GetBloodRequestByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"/api/blood-requests/{id}", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BloodRequestDto>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize blood request response.");
    }

    public async Task<BloodRequestDto> CreateBloodRequestAsync(CreateBloodRequestDto body, Guid operatorId, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/blood-requests")
        {
            Content = JsonContent.Create(body)
        };
        request.Headers.Add("X-Operator-Id", operatorId.ToString());

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BloodRequestDto>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize created blood request response.");
    }

    public async Task CloseBloodRequestAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        var response = await _httpClient.PatchAsync($"/api/blood-requests/{id}/close", emptyContent, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<IReadOnlyList<MedicalCenterDto>> GetMedicalCentersAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync("/api/medical-centers", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<MedicalCenterDto>>(cancellationToken: cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize medical centers response.");
    }

    public async Task AssignOperatorMedicalCenterAsync(Guid operatorId, Guid medicalCenterId, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync($"/api/operators/{operatorId}/medical-center", new AssignMedicalCenterDto(medicalCenterId), cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
