using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonationHistory;

public record GetDonationHistoryQuery(Guid DonorId) : IRequest<DonorDonationHistoryDto>;

public class GetDonationHistoryQueryValidator : AbstractValidator<GetDonationHistoryQuery>
{
    public GetDonationHistoryQueryValidator()
    {
        RuleFor(x => x.DonorId).NotEmpty().WithMessage("Donor Id is required.");
    }
}

public class GetDonationHistoryQueryHandler(IDonorServiceClient donorServiceClient, ILogger<GetDonationHistoryQueryHandler> logger) : IRequestHandler<GetDonationHistoryQuery, DonorDonationHistoryDto>
{
    public async Task<DonorDonationHistoryDto> Handle(GetDonationHistoryQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting donation history for donor {Id}", request.DonorId);
        return await donorServiceClient.GetDonationHistoryAsync(request.DonorId, cancellationToken);
    }
}
