namespace Donnum.Gateway.Application.Models.Donor;

public record DonorDto(Guid Id, string FirstName, string LastName, string Email);
public record CreateDonorDto(string FirstName, string LastName, string Email);
public record UpdateDonorDto(string FirstName, string LastName);

public enum BadgeType { None = 0, FirstDonation = 1, Milestone = 2 } // Mocked enum for the gateway DTO, could be matched to the exact donor service enum
public record DonorBadgeDto(Guid Id, string Name, BadgeType BadgeType, int BadgePoints, DateTime AssignedAt);
public record DonorReliabilityDto(Guid DonorId, int Score, DateTime LastCalculatedAt);

public record DonationHistoryItemDto(Guid Id, Guid DonationRequestId, Guid MedicalCenterId, DateTime DonationDate, DateTime CreatedAt);
public record DonorDonationHistoryDto(Guid DonorId, List<DonationHistoryItemDto> Donations);
public record RegisterAttendanceRequest(Guid DonationRequestId, Guid MedicalCenterId, DateTime DonationDate, bool Attended);
