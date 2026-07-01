using Donnum.Gateway.Application.Models.BloodRequest;

namespace Donnum.Gateway.Application.Contracts;

public interface IBloodRequestServiceClient
{
    Task<PagedBloodRequestResult> GetBloodRequestsAsync(string? status, string? type, Guid? destinationMedicalCenterId, string? donorBloodType, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<PagedBloodRequestResult> GetActiveUrgenciesAsync(string? donorBloodType, int page, int pageSize, CancellationToken cancellationToken = default);
    Task<BloodRequestDto> GetBloodRequestByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<BloodRequestDto> CreateBloodRequestAsync(CreateBloodRequestDto body, Guid operatorId, CancellationToken cancellationToken = default);
    Task CloseBloodRequestAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MedicalCenterDto>> GetMedicalCentersAsync(CancellationToken cancellationToken = default);
    Task AssignOperatorMedicalCenterAsync(Guid operatorId, Guid medicalCenterId, CancellationToken cancellationToken = default);
}
