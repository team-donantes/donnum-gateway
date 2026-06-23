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

public record GetMonthlyEvolutionQuery(string? Period, int? Limit) : IRequest<IReadOnlyList<MonthlyEvolutionDto>>;

public class GetMonthlyEvolutionQueryHandler(
    IMetricServiceClient metricServiceClient, 
    ILogger<GetMonthlyEvolutionQueryHandler> logger) 
    : IRequestHandler<GetMonthlyEvolutionQuery, IReadOnlyList<MonthlyEvolutionDto>>
{
    public async Task<IReadOnlyList<MonthlyEvolutionDto>> Handle(GetMonthlyEvolutionQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting monthly evolution for Period: {Period}, Limit: {Limit}", 
            request.Period, request.Limit);
            
        var internalData = await metricServiceClient.GetEmergencyMetricsAsync(
            request.Period, 
            request.Limit, 
            cancellationToken);
            
        return MetricMapper.MapToMonthlyEvolution(internalData);
    }
}
