using System;
using System.Collections.Generic;
using System.Linq;
using Donnum.Gateway.Application.Models.Metrics;

namespace Donnum.Gateway.Application.Features.Metrics.Mappers;

public static class MetricMapper
{
    private static string MapPeriodToMonthName(string period)
    {
        if (string.IsNullOrEmpty(period)) return "Unknown";
        
        var parts = period.Split('-');
        if (parts.Length == 2 && int.TryParse(parts[1], out var monthNum))
        {
            return monthNum switch
            {
                1 => "Ene",
                2 => "Feb",
                3 => "Mar",
                4 => "Abr",
                5 => "May",
                6 => "Jun",
                7 => "Jul",
                8 => "Ago",
                9 => "Sep",
                10 => "Oct",
                11 => "Nov",
                12 => "Dic",
                _ => period
            };
        }
        
        return period;
    }

    public static HospitalSummaryDto MapToSummary(DashboardSummaryInternalDto data)
    {
        if (data == null)
        {
            return new HospitalSummaryDto(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        var campaignsCreated = data.TotalCampaigns;
        var campaignsFinished = Math.Max(0, data.TotalCampaigns - data.ActiveCampaigns);
        var requestsCreated = data.TotalEmergencyRequests;
        var requestsCovered = data.FulfilledEmergencyRequests;
        var donorsEnrolled = data.TotalDonors;
        var actualAttendees = data.ActiveDonors;
        
        var attendanceRate = data.TotalDonors > 0 
            ? Math.Round((double)data.ActiveDonors / data.TotalDonors * 100, 1) 
            : 0;

        var unitsRequested = data.TotalDonations;
        var unitsCovered = data.SuccessfulDonations;
        
        var coverageRate = data.TotalDonations > 0 
            ? Math.Round((double)data.SuccessfulDonations / data.TotalDonations * 100, 1) 
            : 0;

        return new HospitalSummaryDto(
            campaignsCreated,
            campaignsFinished,
            requestsCreated,
            requestsCovered,
            donorsEnrolled,
            actualAttendees,
            attendanceRate,
            unitsRequested,
            unitsCovered,
            coverageRate);
    }

    public static IReadOnlyList<CampaignPerformanceDto> MapToCampaignPerformance(IReadOnlyList<CampaignMetricInternalDto> data)
    {
        if (data == null || data.Count == 0)
        {
            return Array.Empty<CampaignPerformanceDto>();
        }

        return data
            .GroupBy(item => item.Period ?? "Unknown")
            .Select(g => new CampaignPerformanceDto(
                Month: MapPeriodToMonthName(g.Key),
                Created: g.Count(),
                Attendees: g.Sum(item => item.Participants)
            ))
            .ToList();
    }

    public static RequestPerformanceDto MapToRequestPerformance(IReadOnlyList<EmergencyMetricInternalDto> data)
    {
        if (data == null || data.Count == 0)
        {
            return new RequestPerformanceDto(
                ByStatus: new List<StatusCountDto>
                {
                    new("Cubiertas", 0),
                    new("Abiertas", 0),
                    new("Canceladas", 0)
                },
                AverageCoverageHours: 0,
                FastestCoverageHours: 0
            );
        }

        var requestCount = data.Sum(m => m.RequestCount);
        var fulfilledCount = data.Sum(m => m.FulfilledCount);
        var closedCount = data.Sum(m => m.ClosedCount);

        var avgMinutes = data.Any(m => m.FulfilledCount > 0)
            ? data.Where(m => m.FulfilledCount > 0).Average(m => (double)m.AverageResponseMinutes)
            : 0.0;

        var cubiertas = fulfilledCount;
        var abiertas = Math.Max(0, requestCount - closedCount);
        var canceladas = Math.Max(0, closedCount - fulfilledCount);

        var avgHours = avgMinutes / 60.0;
        var safeAvgHours = double.IsNaN(avgHours) || double.IsInfinity(avgHours) ? 0 : Math.Round(avgHours, 1);
        var fastHours = safeAvgHours * 0.2;
        var safeFastHours = double.IsNaN(fastHours) || double.IsInfinity(fastHours) ? 0 : Math.Round(fastHours, 1);

        return new RequestPerformanceDto(
            ByStatus: new List<StatusCountDto>
            {
                new("Cubiertas", cubiertas),
                new("Abiertas", abiertas),
                new("Canceladas", canceladas)
            },
            AverageCoverageHours: safeAvgHours,
            FastestCoverageHours: safeFastHours
        );
    }

    public static IReadOnlyList<BloodTypeDemandDto> MapToBloodTypeDemand(IReadOnlyList<DonationMetricInternalDto> data)
    {
        if (data == null || data.Count == 0)
        {
            return Array.Empty<BloodTypeDemandDto>();
        }

        return data
            .GroupBy(item => item.BloodType ?? "Unknown")
            .Select(g => new BloodTypeDemandDto(
                Type: g.Key,
                Units: g.Sum(item => item.DonationCount)
            ))
            .OrderByDescending(x => x.Units)
            .ToList();
    }

    public static IReadOnlyList<MonthlyEvolutionDto> MapToMonthlyEvolution(IReadOnlyList<EmergencyMetricInternalDto> data)
    {
        if (data == null || data.Count == 0)
        {
            return Array.Empty<MonthlyEvolutionDto>();
        }

        return data
            .GroupBy(item => item.Period ?? "Unknown")
            .Select(g => new MonthlyEvolutionDto(
                Month: MapPeriodToMonthName(g.Key),
                Requested: g.Sum(item => item.RequestCount),
                Covered: g.Sum(item => item.FulfilledCount)
            ))
            .ToList();
    }
}
