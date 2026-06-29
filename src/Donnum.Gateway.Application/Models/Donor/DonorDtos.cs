namespace Donnum.Gateway.Application.Models.Donor;

public record DonorDto(
    Guid Id,
    Guid? AuthUserId = null,
    string? FirstName = null,
    string? LastName = null,
    string? PhoneNumber = null,
    string? BloodGroup = null,
    string? RhFactor = null,
    string? Street = null,
    string? City = null,
    string? Province = null,
    int? Points = null,
    int? Reliability = null,
    DateTime? CreatedAt = null,
    DateTime? UpdatedAt = null,
    bool? Attended = null
);
public record CreateDonorDto(
    Guid AuthUserId,
    string PhoneNumber,
    string BloodGroup,
    string RhFactor,
    int Gender,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude
);
public record UpdateDonorDto(string? Street, string City, string Province, string PhoneNumber);

public enum BadgeType { None = 0, FirstDonation = 1, Milestone = 2 } // Mocked enum for the gateway DTO, could be matched to the exact donor service enum
public record DonorBadgeDto(Guid Id, string Name, BadgeType BadgeType, int BadgePoints, DateTime AssignedAt);
public record DonorReliabilityDto(Guid DonorId, int Score, DateTime LastCalculatedAt);

public record DonationHistoryItemDto(Guid Id, Guid DonationRequestId, Guid MedicalCenterId, DateTime DonationDate, DateTime CreatedAt);
public record ParticipationDto(Guid Id, Guid DonationRequestId, int Status, DateTime RegisteredAt);
public record DonorDonationHistoryDto(Guid DonorId, List<DonationHistoryItemDto> Donations, List<ParticipationDto> Participations);
public record RegisterAttendanceRequest(Guid DonationRequestId, Guid MedicalCenterId, DateTime DonationDate, bool Attended);
