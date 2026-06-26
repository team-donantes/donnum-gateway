using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Commands.CreateDonor;

public record CreateDonorCommand(
    Guid AuthUserId,
    string BloodGroup,
    string RhFactor,
    int Gender,
    string? Street,
    string City,
    string Province,
    decimal Latitude,
    decimal Longitude
) : IRequest<DonorDto>;

public class CreateDonorCommandValidator : AbstractValidator<CreateDonorCommand>
{
    public CreateDonorCommandValidator()
    {
        RuleFor(x => x.BloodGroup).NotEmpty().WithMessage("Blood group is required.");
        RuleFor(x => x.RhFactor).NotEmpty().WithMessage("Rh factor is required.");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
        RuleFor(x => x.Province).NotEmpty().WithMessage("Province is required.");
    }
}

public class CreateDonorCommandHandler(IDonorServiceClient donorServiceClient, ILogger<CreateDonorCommandHandler> logger) : IRequestHandler<CreateDonorCommand, DonorDto>
{
    public async Task<DonorDto> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating donor profile for user {AuthUserId}", request.AuthUserId);
        var createDto = new CreateDonorDto(
            request.AuthUserId,
            request.BloodGroup,
            request.RhFactor,
            request.Gender,
            request.Street,
            request.City,
            request.Province,
            request.Latitude,
            request.Longitude
        );
        return await donorServiceClient.CreateDonorAsync(createDto, cancellationToken);
    }
}
