using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Domain.Exceptions;
using MediatR;

namespace Donnum.Gateway.Application.Features.Donations.Commands.CreateParticipation;

public sealed class CreateParticipationCommandHandler : IRequestHandler<CreateParticipationCommand, bool>
{
    private readonly IDonorServiceClient _donorServiceClient;
    private readonly IBloodRequestServiceClient _bloodRequestServiceClient;

    public CreateParticipationCommandHandler(
        IDonorServiceClient donorServiceClient,
        IBloodRequestServiceClient bloodRequestServiceClient)
    {
        _donorServiceClient = donorServiceClient;
        _bloodRequestServiceClient = bloodRequestServiceClient;
    }

    public async Task<bool> Handle(CreateParticipationCommand request, CancellationToken cancellationToken)
    {
        var bloodRequest = await _bloodRequestServiceClient.GetBloodRequestByIdAsync(request.Request.DonationRequestId, cancellationToken);
        var requestedTypes = bloodRequest.RequestedBloodTypes.Select(bt => bt.BloodType).ToList();

        request.Request.RequestedBloodTypes = requestedTypes;

        return await _donorServiceClient.CreateParticipationAsync(request.DonorId, request.Request, cancellationToken);
    }
}
