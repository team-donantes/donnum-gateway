using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.BloodRequests.Queries.GetActiveUrgencies;

public record GetActiveUrgenciesQuery(int Page = 1, int PageSize = 10) : IRequest<PagedBloodRequestResult>;

public class GetActiveUrgenciesQueryHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<GetActiveUrgenciesQueryHandler> logger) : IRequestHandler<GetActiveUrgenciesQuery, PagedBloodRequestResult>
{
    public async Task<PagedBloodRequestResult> Handle(GetActiveUrgenciesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting active urgencies");
        return await bloodRequestServiceClient.GetActiveUrgenciesAsync(request.Page, request.PageSize, cancellationToken);
    }
}
