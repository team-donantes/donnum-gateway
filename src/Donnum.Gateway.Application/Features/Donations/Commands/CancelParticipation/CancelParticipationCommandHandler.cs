using Donnum.Gateway.Application.Contracts;
using MediatR;

namespace Donnum.Gateway.Application.Features.Donations.Commands.CancelParticipation;

public sealed class CancelParticipationCommandHandler : IRequestHandler<CancelParticipationCommand, bool>
{
    private readonly IDonorServiceClient _donorServiceClient;

    public CancelParticipationCommandHandler(IDonorServiceClient donorServiceClient)
    {
        _donorServiceClient = donorServiceClient;
    }

    public async Task<bool> Handle(CancelParticipationCommand request, CancellationToken cancellationToken)
    {
        return await _donorServiceClient.CancelParticipationAsync(request.DonorId, request.DonationRequestId, cancellationToken);
    }
}
