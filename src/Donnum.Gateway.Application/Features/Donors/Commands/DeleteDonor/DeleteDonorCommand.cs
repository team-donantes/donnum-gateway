using Donnum.Gateway.Application.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Commands.DeleteDonor;

public record DeleteDonorCommand(Guid Id) : IRequest<bool>;

public class DeleteDonorCommandValidator : AbstractValidator<DeleteDonorCommand>
{
    public DeleteDonorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class DeleteDonorCommandHandler(IDonorServiceClient donorServiceClient, ILogger<DeleteDonorCommandHandler> logger) : IRequestHandler<DeleteDonorCommand, bool>
{
    public async Task<bool> Handle(DeleteDonorCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting donor {Id}", request.Id);
        return await donorServiceClient.DeleteDonorAsync(request.Id, cancellationToken);
    }
}
