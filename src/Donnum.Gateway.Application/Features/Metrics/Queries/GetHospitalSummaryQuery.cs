using System;
using System.Threading;
using System.Threading.Tasks;
using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Features.Metrics.Mappers;
using Donnum.Gateway.Application.Models.Metrics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Metrics.Queries;

public record GetHospitalSummaryQuery(string? Period, DateOnly? From, DateOnly? To) : IRequest<HospitalSummaryDto>;

public class GetHospitalSummaryQueryHandler(
    IMetricServiceClient metricServiceClient, 
    ILogger<GetHospitalSummaryQueryHandler> logger) 
    : IRequestHandler<GetHospitalSummaryQuery, HospitalSummaryDto>
{
    public async Task<HospitalSummaryDto> Handle(GetHospitalSummaryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting hospital metrics summary for Period: {Period}, From: {From}, To: {To}", 
            request.Period, request.From, request.To);
            
        var internalData = await metricServiceClient.GetDashboardSummaryAsync(
            request.Period, 
            request.From, 
            request.To, 
            cancellationToken);
            
        return MetricMapper.MapToSummary(internalData);
    }
}
