namespace Donnum.Gateway.Application.Models.BloodRequest;

public record BloodRequestDto(
    Guid Id,
    Guid SourceMedicalCenterId,
    Guid? DestinationMedicalCenterId,
    Guid CreatedByOperatorId,
    string Title,
    string? Description,
    string Type,
    string Status,
    float? RadiusKm,
    DateTime? StartsAt,
    DateTime? EndsAt,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<RequestedBloodTypeDto> RequestedBloodTypes,
    List<CampaignLocationDto> CampaignLocations,
    MedicalCenterDto? DestinationMedicalCenter
);

public record RequestedBloodTypeDto(Guid Id, string BloodType, int? UnitsRequired, int? UnitsCovered);

public record CampaignLocationDto(Guid Id, string Name, string Address, decimal Latitude, decimal Longitude);

public record MedicalCenterDto(Guid Id, string Name, decimal Latitude, decimal Longitude, string Address, string OpeningHours);

public record PagedBloodRequestResult(
    List<BloodRequestDto> Data,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages
);

public record CreateBloodRequestDto(
    Guid? DestinationMedicalCenterId,
    string Title,
    string? Description,
    string Type,
    float? RadiusKm,
    DateTime? StartsAt,
    DateTime? EndsAt,
    List<CreateBloodTypeDto> BloodTypes,
    List<CreateLocationDto>? Locations
);

public record CreateBloodTypeDto(string BloodType, int? UnitsRequired);

public record CreateLocationDto(string Name, string Address, decimal Latitude, decimal Longitude);

public record AssignMedicalCenterDto(Guid MedicalCenterId);
