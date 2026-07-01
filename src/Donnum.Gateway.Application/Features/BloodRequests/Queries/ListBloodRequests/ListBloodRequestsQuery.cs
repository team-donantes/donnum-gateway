using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.BloodRequests.Queries.ListBloodRequests;

public record ListBloodRequestsQuery(string? Status, string? Type, Guid? DestinationMedicalCenterId, string? DonorBloodType, int Page, int PageSize) : IRequest<PagedBloodRequestResult>;

public class ListBloodRequestsQueryHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<ListBloodRequestsQueryHandler> logger) : IRequestHandler<ListBloodRequestsQuery, PagedBloodRequestResult>
{
    public async Task<PagedBloodRequestResult> Handle(ListBloodRequestsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Listing blood requests — Type: {Type}, Status: {Status}, Page: {Page}", request.Type, request.Status, request.Page);
        return await bloodRequestServiceClient.GetBloodRequestsAsync(request.Status, request.Type, request.DestinationMedicalCenterId, request.DonorBloodType, request.Page, request.PageSize, cancellationToken);
    }
}
