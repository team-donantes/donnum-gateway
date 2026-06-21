using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonorReliability;

public record GetDonorReliabilityQuery(Guid DonorId) : IRequest<DonorReliabilityDto>;

public class GetDonorReliabilityQueryValidator : AbstractValidator<GetDonorReliabilityQuery>
{
    public GetDonorReliabilityQueryValidator()
    {
        RuleFor(x => x.DonorId).NotEmpty().WithMessage("Donor Id is required.");
    }
}

public class GetDonorReliabilityQueryHandler(IDonorServiceClient donorServiceClient, ILogger<GetDonorReliabilityQueryHandler> logger) : IRequestHandler<GetDonorReliabilityQuery, DonorReliabilityDto>
{
    public async Task<DonorReliabilityDto> Handle(GetDonorReliabilityQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting reliability for donor {Id}", request.DonorId);
        return await donorServiceClient.GetDonorReliabilityAsync(request.DonorId, cancellationToken);
    }
}
