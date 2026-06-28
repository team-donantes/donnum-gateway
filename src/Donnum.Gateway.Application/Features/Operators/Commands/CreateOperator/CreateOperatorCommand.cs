using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.User;
using Donnum.Gateway.Domain.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Operators.Commands.CreateOperator;

public record CreateOperatorCommandResult(Guid OperatorId, string CorporateEmail);

public record CreateOperatorCommand(
    string Token,
    string FirstName,
    string LastName,
    string Password,
    Guid HospitalId,
    string PersonalEmail
) : IRequest<CreateOperatorCommandResult>;

public class CreateOperatorCommandValidator : AbstractValidator<CreateOperatorCommand>
{
    public CreateOperatorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.HospitalId).NotEmpty().WithMessage("HospitalId is required.");
        RuleFor(x => x.PersonalEmail).NotEmpty().EmailAddress().WithMessage("Valid personal email is required.");
    }
}

public class CreateOperatorCommandHandler(
    IUserServiceClient userServiceClient,
    IBloodRequestServiceClient bloodRequestServiceClient,
    ILogger<CreateOperatorCommandHandler> logger) : IRequestHandler<CreateOperatorCommand, CreateOperatorCommandResult>
{
    public async Task<CreateOperatorCommandResult> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating operator identity for {Email}", request.PersonalEmail);

        var identityRequest = new CreateOperatorIdentityRequestDto(
            request.FirstName,
            request.LastName,
            request.PersonalEmail,
            request.Password
        );

        var identityResponse = await userServiceClient.CreateOperatorIdentityAsync(request.Token, identityRequest, cancellationToken);
        var operatorId = identityResponse.CredentialId;
        var corporateEmail = identityResponse.CorporateEmail;

        logger.LogInformation("Operator identity created with ID {OperatorId}. Assigning to Hospital {HospitalId}", operatorId, request.HospitalId);

        try
        {
            await bloodRequestServiceClient.AssignOperatorMedicalCenterAsync(operatorId, request.HospitalId, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to assign operator {OperatorId} to hospital {HospitalId}. Operator identity was created but remains unassigned.", operatorId, request.HospitalId);
            throw new DomainException($"Operator identity created but failed to assign to medical center: {ex.Message}");
        }

        return new CreateOperatorCommandResult(operatorId, corporateEmail);
    }
}
