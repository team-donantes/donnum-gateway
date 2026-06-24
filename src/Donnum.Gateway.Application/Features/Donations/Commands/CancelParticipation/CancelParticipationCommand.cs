using MediatR;

namespace Donnum.Gateway.Application.Features.Donations.Commands.CancelParticipation;

public record CancelParticipationCommand(Guid DonorId, Guid DonationRequestId) : IRequest<bool>;
