using System;
using System.Collections.Generic;

namespace Donnum.Gateway.Application.Models.Metrics;

// --- DTOs Internos (Contrato con metric-service) ---

public sealed record DashboardSummaryInternalDto(
    DateOnly SnapshotDate,
    int TotalDonors,
    int ActiveDonors,
    int TotalDonations,
    int SuccessfulDonations,
    decimal DonationSuccessRate,
    int TotalCampaigns,
    int ActiveCampaigns,
    int TotalEmergencyRequests,
    int FulfilledEmergencyRequests,
    int ClosedEmergencyRequests,
    decimal EmergencyFulfillmentRate,
    IReadOnlyList<DonationTrendInternalDto> DonationTrends,
    IReadOnlyList<DonorTrendInternalDto> DonorTrends,
    IReadOnlyList<CampaignPerformanceInternalDto> TopCampaigns,
    IReadOnlyList<EmergencyTrendInternalDto> EmergencyTrends);

public sealed record DonationTrendInternalDto(string Period, string BloodType, int DonationCount);
public sealed record DonorTrendInternalDto(string Period, int NewDonors, int ActiveDonors, int InactiveDonors);
public sealed record CampaignPerformanceInternalDto(string Period, Guid CampaignId, int Participants, int DonationsGenerated);
public sealed record EmergencyTrendInternalDto(string Period, int RequestCount, int FulfilledCount, int ClosedCount, decimal AverageResponseMinutes);

public sealed record CampaignMetricInternalDto(Guid Id, string Period, Guid CampaignId, int Participants, int DonationsGenerated, DateTimeOffset CreatedAt);
public sealed record EmergencyMetricInternalDto(Guid Id, string Period, int RequestCount, int FulfilledCount, int ClosedCount, decimal AverageResponseMinutes, DateTimeOffset CreatedAt);
public sealed record DonationMetricInternalDto(Guid Id, string Period, string BloodType, int DonationCount, DateTimeOffset CreatedAt);


// --- DTOs Públicos (Contrato con el Frontend) ---

public sealed record HospitalSummaryDto(
    int CampaignsCreated,
    int CampaignsFinished,
    int RequestsCreated,
    int RequestsCovered,
    int DonorsEnrolled,
    int ActualAttendees,
    double AttendanceRate,
    int UnitsRequested,
    int UnitsCovered,
    double CoverageRate);

public sealed record CampaignPerformanceDto(
    string Month,
    int Created,
    int Attendees);

public sealed record StatusCountDto(
    string Status,
    int Count);

public sealed record RequestPerformanceDto(
    IReadOnlyList<StatusCountDto> ByStatus,
    double AverageCoverageHours,
    double FastestCoverageHours);

public sealed record BloodTypeDemandDto(
    string Type,
    int Units);

public sealed record MonthlyEvolutionDto(
    string Month,
    int Requested,
    int Covered);
