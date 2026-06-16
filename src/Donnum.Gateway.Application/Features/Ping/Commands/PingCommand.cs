using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Ping.Commands;

public record PingCommand(string Message) : IRequest<string>;

public class PingCommandValidator : AbstractValidator<PingCommand>
{
    public PingCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(100).WithMessage("Message cannot exceed 100 characters.");
    }
}

public class PingCommandHandler(ILogger<PingCommandHandler> logger) : IRequestHandler<PingCommand, string>
{
    public Task<string> Handle(PingCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Processing ping command with message: {Message}", request.Message);
        return Task.FromResult($"Pong: {request.Message}");
    }
}
