using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Donnum.Gateway.Application.Models.Metrics;

namespace Donnum.Gateway.Application.Contracts;

public interface IMetricServiceClient
{
    Task<DashboardSummaryInternalDto> GetDashboardSummaryAsync(string? period, DateOnly? from, DateOnly? to, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CampaignMetricInternalDto>> GetCampaignMetricsAsync(string? period, Guid? campaignId, int? limit, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EmergencyMetricInternalDto>> GetEmergencyMetricsAsync(string? period, int? limit, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DonationMetricInternalDto>> GetDonationMetricsAsync(string? period, string? bloodType, int? limit, CancellationToken cancellationToken = default);
}
