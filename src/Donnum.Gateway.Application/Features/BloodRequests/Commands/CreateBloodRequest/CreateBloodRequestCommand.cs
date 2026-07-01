using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.BloodRequests.Commands.CreateBloodRequest;

public record CreateBloodRequestCommand(
    Guid OperatorId,
    Guid? DestinationMedicalCenterId,
    string Title,
    string? Description,
    string Type,
    float? RadiusKm,
    DateTime? StartsAt,
    DateTime? EndsAt,
    List<CreateBloodTypeDto> BloodTypes,
    List<CreateLocationDto>? Locations
) : IRequest<BloodRequestDto>;

public class CreateBloodRequestCommandValidator : AbstractValidator<CreateBloodRequestCommand>
{
    public CreateBloodRequestCommandValidator()
    {
        RuleFor(x => x.OperatorId).NotEmpty().WithMessage("OperatorId is required.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required.");
        RuleFor(x => x.BloodTypes).NotEmpty().WithMessage("At least one blood type is required.");
    }
}

public class CreateBloodRequestCommandHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<CreateBloodRequestCommandHandler> logger) : IRequestHandler<CreateBloodRequestCommand, BloodRequestDto>
{
    public async Task<BloodRequestDto> Handle(CreateBloodRequestCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating blood request of type {Type} for operator {OperatorId}", request.Type, request.OperatorId);
        var dto = new CreateBloodRequestDto(request.DestinationMedicalCenterId, request.Title, request.Description, request.Type, request.RadiusKm, request.StartsAt, request.EndsAt, request.BloodTypes, request.Locations);
        return await bloodRequestServiceClient.CreateBloodRequestAsync(dto, request.OperatorId, cancellationToken);
    }
}
