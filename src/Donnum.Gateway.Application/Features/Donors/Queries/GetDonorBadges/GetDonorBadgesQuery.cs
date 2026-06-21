using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonorBadges;

public record GetDonorBadgesQuery(Guid DonorId) : IRequest<IReadOnlyList<DonorBadgeDto>>;

public class GetDonorBadgesQueryValidator : AbstractValidator<GetDonorBadgesQuery>
{
    public GetDonorBadgesQueryValidator()
    {
        RuleFor(x => x.DonorId).NotEmpty().WithMessage("Donor Id is required.");
    }
}

public class GetDonorBadgesQueryHandler(IDonorServiceClient donorServiceClient, ILogger<GetDonorBadgesQueryHandler> logger) : IRequestHandler<GetDonorBadgesQuery, IReadOnlyList<DonorBadgeDto>>
{
    public async Task<IReadOnlyList<DonorBadgeDto>> Handle(GetDonorBadgesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting badges for donor {Id}", request.DonorId);
        return await donorServiceClient.GetDonorBadgesAsync(request.DonorId, cancellationToken);
    }
}
