using Donnum.Gateway.Application.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.BloodRequests.Commands.CloseBloodRequest;

public record CloseBloodRequestCommand(Guid Id) : IRequest;

public class CloseBloodRequestCommandValidator : AbstractValidator<CloseBloodRequestCommand>
{
    public CloseBloodRequestCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class CloseBloodRequestCommandHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<CloseBloodRequestCommandHandler> logger) : IRequestHandler<CloseBloodRequestCommand>
{
    public async Task Handle(CloseBloodRequestCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Closing blood request {Id}", request.Id);
        await bloodRequestServiceClient.CloseBloodRequestAsync(request.Id, cancellationToken);
    }
}
