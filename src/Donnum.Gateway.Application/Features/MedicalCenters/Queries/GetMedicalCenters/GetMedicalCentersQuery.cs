using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.MedicalCenters.Queries.GetMedicalCenters;

public record GetMedicalCentersQuery : IRequest<IReadOnlyList<MedicalCenterDto>>;

public class GetMedicalCentersQueryHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<GetMedicalCentersQueryHandler> logger) : IRequestHandler<GetMedicalCentersQuery, IReadOnlyList<MedicalCenterDto>>
{
    public async Task<IReadOnlyList<MedicalCenterDto>> Handle(GetMedicalCentersQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting medical centers");
        return await bloodRequestServiceClient.GetMedicalCentersAsync(cancellationToken);
    }
}
