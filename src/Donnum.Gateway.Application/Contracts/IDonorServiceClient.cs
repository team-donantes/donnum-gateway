using Donnum.Gateway.Application.Models.Donor;

namespace Donnum.Gateway.Application.Contracts;

public interface IDonorServiceClient
{
    Task<DonorDto> GetDonorAsync(Guid id, CancellationToken cancellationToken = default);
    Task<DonorDto> CreateDonorAsync(CreateDonorDto request, CancellationToken cancellationToken = default);
    Task<DonorDto> UpdateDonorAsync(Guid id, UpdateDonorDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteDonorAsync(Guid id, CancellationToken cancellationToken = default);
}
