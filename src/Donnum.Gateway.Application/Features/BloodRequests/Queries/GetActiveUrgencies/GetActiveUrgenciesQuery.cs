using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.BloodRequests.Queries.GetActiveUrgencies;

public record GetActiveUrgenciesQuery : IRequest<IReadOnlyList<BloodRequestDto>>;

public class GetActiveUrgenciesQueryHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<GetActiveUrgenciesQueryHandler> logger) : IRequestHandler<GetActiveUrgenciesQuery, IReadOnlyList<BloodRequestDto>>
{
    public async Task<IReadOnlyList<BloodRequestDto>> Handle(GetActiveUrgenciesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting active urgencies");
        return await bloodRequestServiceClient.GetActiveUrgenciesAsync(cancellationToken);
    }
}
