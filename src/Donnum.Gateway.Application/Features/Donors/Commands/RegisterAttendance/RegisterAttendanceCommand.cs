using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Commands.RegisterAttendance;

public record RegisterAttendanceCommand(Guid DonorId, RegisterAttendanceRequest Payload) : IRequest<bool>;

public class RegisterAttendanceCommandValidator : AbstractValidator<RegisterAttendanceCommand>
{
    public RegisterAttendanceCommandValidator()
    {
        RuleFor(x => x.DonorId).NotEmpty().WithMessage("Donor Id is required.");
        RuleFor(x => x.Payload).NotNull().WithMessage("Payload is required.");
        RuleFor(x => x.Payload.DonationRequestId).NotEmpty().WithMessage("Donation Request Id is required.");
        RuleFor(x => x.Payload.MedicalCenterId).NotEmpty().WithMessage("Medical Center Id is required.");
    }
}

public class RegisterAttendanceCommandHandler(IDonorServiceClient donorServiceClient, ILogger<RegisterAttendanceCommandHandler> logger) : IRequestHandler<RegisterAttendanceCommand, bool>
{
    public async Task<bool> Handle(RegisterAttendanceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Registering attendance for donor {Id}", request.DonorId);
        return await donorServiceClient.RegisterAttendanceAsync(request.DonorId, request.Payload, cancellationToken);
    }
}
