using Donnum.Gateway.Application.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Operators.Commands.AssignMedicalCenter;

public record AssignOperatorMedicalCenterCommand(Guid OperatorId, Guid MedicalCenterId) : IRequest;

public class AssignOperatorMedicalCenterCommandValidator : AbstractValidator<AssignOperatorMedicalCenterCommand>
{
    public AssignOperatorMedicalCenterCommandValidator()
    {
        RuleFor(x => x.OperatorId).NotEmpty().WithMessage("OperatorId is required.");
        RuleFor(x => x.MedicalCenterId).NotEmpty().WithMessage("MedicalCenterId is required.");
    }
}

public class AssignOperatorMedicalCenterCommandHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<AssignOperatorMedicalCenterCommandHandler> logger) : IRequestHandler<AssignOperatorMedicalCenterCommand>
{
    public async Task Handle(AssignOperatorMedicalCenterCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning operator {OperatorId} to medical center {MedicalCenterId}", request.OperatorId, request.MedicalCenterId);
        await bloodRequestServiceClient.AssignOperatorMedicalCenterAsync(request.OperatorId, request.MedicalCenterId, cancellationToken);
    }
}
