using Donnum.Gateway.Application.Models.Donor;

namespace Donnum.Gateway.Application.Contracts;

public interface IDonorServiceClient
{
    Task<DonorDto> GetDonorAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DonorDto> CreateDonorAsync(CreateDonorDto request, CancellationToken cancellationToken = default);
    Task UpdateDonorAsync(Guid id, UpdateDonorDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteDonorAsync(Guid id, CancellationToken cancellationToken = default);

    Task<DonorDonationHistoryDto> GetDonationHistoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DonorBadgeDto>> GetDonorBadgesAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DonorReliabilityDto> GetDonorReliabilityAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> RegisterAttendanceAsync(Guid id, RegisterAttendanceRequest request, CancellationToken cancellationToken = default);

    Task<bool> CreateParticipationAsync(Guid donorId, CreateParticipationDto request, CancellationToken cancellationToken = default);
    Task<bool> CancelParticipationAsync(Guid donorId, Guid requestId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DonorDto>> GetDonorsByRequestAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<Guid?> GetDonorIdByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
