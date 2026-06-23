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

public record GetBloodTypeDemandQuery(string? Period, string? BloodType, int? Limit) : IRequest<IReadOnlyList<BloodTypeDemandDto>>;

public class GetBloodTypeDemandQueryHandler(
    IMetricServiceClient metricServiceClient, 
    ILogger<GetBloodTypeDemandQueryHandler> logger) 
    : IRequestHandler<GetBloodTypeDemandQuery, IReadOnlyList<BloodTypeDemandDto>>
{
    public async Task<IReadOnlyList<BloodTypeDemandDto>> Handle(GetBloodTypeDemandQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting blood type demand for Period: {Period}, BloodType: {BloodType}", 
            request.Period, request.BloodType);
            
        var internalData = await metricServiceClient.GetDonationMetricsAsync(
            request.Period, 
            request.BloodType, 
            request.Limit, 
            cancellationToken);
            
        return MetricMapper.MapToBloodTypeDemand(internalData);
    }
}
