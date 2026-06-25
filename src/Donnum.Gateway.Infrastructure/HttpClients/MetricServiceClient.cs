using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Exceptions;
using Donnum.Gateway.Application.Models.Metrics;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Infrastructure.HttpClients;

public class MetricServiceClient : IMetricServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<MetricServiceClient> _logger;

    public MetricServiceClient(HttpClient httpClient, ILogger<MetricServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<DashboardSummaryInternalDto> GetDashboardSummaryAsync(
        string? period, 
        DateOnly? from, 
        DateOnly? to, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(period)) queryParams.Add($"period={period.Trim()}");
            if (from.HasValue) queryParams.Add($"from={from.Value:yyyy-MM-dd}");
            if (to.HasValue) queryParams.Add($"to={to.Value:yyyy-MM-dd}");
            
            var url = "/metrics/dashboard" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "");
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Downstream metric-service dashboard endpoint returned status {StatusCode}", response.StatusCode);
                throw new MetricServiceUnavailableException("El servicio de métricas no respondió correctamente.");
            }

            return await response.Content.ReadFromJsonAsync<DashboardSummaryInternalDto>(cancellationToken: cancellationToken)
                   ?? throw new MetricServiceUnavailableException("Error de deserialización en la respuesta del servicio de métricas.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to downstream metric-service dashboard endpoint");
            throw new MetricServiceUnavailableException("El servicio de métricas no está disponible actualmente.", ex);
        }
    }

    public async Task<IReadOnlyList<CampaignMetricInternalDto>> GetCampaignMetricsAsync(
        string? period, 
        Guid? campaignId, 
        int? limit, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(period)) queryParams.Add($"period={period.Trim()}");
            if (campaignId.HasValue) queryParams.Add($"campaignId={campaignId.Value}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            
            var url = "/metrics/campaigns" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "");
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Downstream metric-service campaigns endpoint returned status {StatusCode}", response.StatusCode);
                throw new MetricServiceUnavailableException("El servicio de métricas no respondió correctamente.");
            }

            return await response.Content.ReadFromJsonAsync<List<CampaignMetricInternalDto>>(cancellationToken: cancellationToken)
                   ?? throw new MetricServiceUnavailableException("Error de deserialización en la respuesta del servicio de métricas.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to downstream metric-service campaigns endpoint");
            throw new MetricServiceUnavailableException("El servicio de métricas no está disponible actualmente.", ex);
        }
    }

    public async Task<IReadOnlyList<EmergencyMetricInternalDto>> GetEmergencyMetricsAsync(
        string? period, 
        int? limit, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(period)) queryParams.Add($"period={period.Trim()}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            
            var url = "/metrics/emergencies" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "");
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Downstream metric-service emergencies endpoint returned status {StatusCode}", response.StatusCode);
                throw new MetricServiceUnavailableException("El servicio de métricas no respondió correctamente.");
            }

            return await response.Content.ReadFromJsonAsync<List<EmergencyMetricInternalDto>>(cancellationToken: cancellationToken)
                   ?? throw new MetricServiceUnavailableException("Error de deserialización en la respuesta del servicio de métricas.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to downstream metric-service emergencies endpoint");
            throw new MetricServiceUnavailableException("El servicio de métricas no está disponible actualmente.", ex);
        }
    }

    public async Task<IReadOnlyList<DonationMetricInternalDto>> GetDonationMetricsAsync(
        string? period, 
        string? bloodType, 
        int? limit, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var queryParams = new List<string>();
            if (!string.IsNullOrWhiteSpace(period)) queryParams.Add($"period={period.Trim()}");
            if (!string.IsNullOrWhiteSpace(bloodType)) queryParams.Add($"bloodType={Uri.EscapeDataString(bloodType.Trim())}");
            if (limit.HasValue) queryParams.Add($"limit={limit.Value}");
            
            var url = "/metrics/donations" + (queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "");
            
            var response = await _httpClient.GetAsync(url, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Downstream metric-service donations endpoint returned status {StatusCode}", response.StatusCode);
                throw new MetricServiceUnavailableException("El servicio de métricas no respondió correctamente.");
            }

            return await response.Content.ReadFromJsonAsync<List<DonationMetricInternalDto>>(cancellationToken: cancellationToken)
                   ?? throw new MetricServiceUnavailableException("Error de deserialización en la respuesta del servicio de métricas.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to connect to downstream metric-service donations endpoint");
            throw new MetricServiceUnavailableException("El servicio de métricas no está disponible actualmente.", ex);
        }
    }
}
