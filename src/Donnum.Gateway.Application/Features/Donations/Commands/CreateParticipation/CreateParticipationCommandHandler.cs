using Donnum.Gateway.Application.Contracts;
using MediatR;

namespace Donnum.Gateway.Application.Features.Donations.Commands.CreateParticipation;

public sealed class CreateParticipationCommandHandler : IRequestHandler<CreateParticipationCommand, bool>
{
    private readonly IDonorServiceClient _donorServiceClient;

    public CreateParticipationCommandHandler(IDonorServiceClient donorServiceClient)
    {
        _donorServiceClient = donorServiceClient;
    }

    public async Task<bool> Handle(CreateParticipationCommand request, CancellationToken cancellationToken)
    {
        return await _donorServiceClient.CreateParticipationAsync(request.DonorId, request.Request, cancellationToken);
    }
}
