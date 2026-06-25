using System;
using System.Threading;
using System.Threading.Tasks;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Features.Metrics.Mappers;
using Donnum.Gateway.Application.Models.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Metrics.Queries;

public record GetRequestPerformanceQuery(string? Period, int? Limit) : IRequest<RequestPerformanceDto>;

public class GetRequestPerformanceQueryHandler(
    IMetricServiceClient metricServiceClient, 
    ILogger<GetRequestPerformanceQueryHandler> logger) 
    : IRequestHandler<GetRequestPerformanceQuery, RequestPerformanceDto>
{
    public async Task<RequestPerformanceDto> Handle(GetRequestPerformanceQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting request performance for Period: {Period}, Limit: {Limit}", 
            request.Period, request.Limit);
            
        var internalData = await metricServiceClient.GetEmergencyMetricsAsync(
            request.Period, 
            request.Limit, 
            cancellationToken);
            
        return MetricMapper.MapToRequestPerformance(internalData);
    }
}
