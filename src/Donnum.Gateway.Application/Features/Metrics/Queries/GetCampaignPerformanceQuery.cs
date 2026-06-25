using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Features.Metrics.Mappers;
using Donnum.Gateway.Application.Models.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Metrics.Queries;

public record GetCampaignPerformanceQuery(string? Period, Guid? CampaignId, int? Limit) : IRequest<IReadOnlyList<CampaignPerformanceDto>>;

public class GetCampaignPerformanceQueryHandler(
    IMetricServiceClient metricServiceClient, 
    ILogger<GetCampaignPerformanceQueryHandler> logger) 
    : IRequestHandler<GetCampaignPerformanceQuery, IReadOnlyList<CampaignPerformanceDto>>
{
    public async Task<IReadOnlyList<CampaignPerformanceDto>> Handle(GetCampaignPerformanceQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting campaign performance for Period: {Period}, CampaignId: {CampaignId}", 
            request.Period, request.CampaignId);
            
        var internalData = await metricServiceClient.GetCampaignMetricsAsync(
            request.Period, 
            request.CampaignId, 
            request.Limit, 
            cancellationToken);
            
        return MetricMapper.MapToCampaignPerformance(internalData);
    }
}
