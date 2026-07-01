using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Commands.UpdateDonor;

public record UpdateDonorCommand(Guid Id, string? Street, string City, string Province, string PhoneNumber) : IRequest;

public class UpdateDonorCommandValidator : AbstractValidator<UpdateDonorCommand>
{
    public UpdateDonorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.Province).NotEmpty().WithMessage("Province is required.");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("PhoneNumber is required.");
    }
}

public class UpdateDonorCommandHandler(IDonorServiceClient donorServiceClient, ILogger<UpdateDonorCommandHandler> logger) : IRequestHandler<UpdateDonorCommand>
{
    public async Task Handle(UpdateDonorCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating donor {Id}", request.Id);
        var updateDto = new UpdateDonorDto(request.Street, request.City, request.Province, request.PhoneNumber);
        await donorServiceClient.UpdateDonorAsync(request.Id, updateDto, cancellationToken);
    }
}
