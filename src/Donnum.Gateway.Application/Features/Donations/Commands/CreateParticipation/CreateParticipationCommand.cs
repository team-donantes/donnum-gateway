using MediatR;
using Donnum.Gateway.Application.Models.Donor;

namespace Donnum.Gateway.Application.Features.Donations.Commands.CreateParticipation;

public record CreateParticipationCommand(Guid DonorId, CreateParticipationDto Request) : IRequest<bool>;
