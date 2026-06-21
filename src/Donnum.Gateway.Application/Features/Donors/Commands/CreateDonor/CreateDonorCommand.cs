using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Commands.CreateDonor;

public record CreateDonorCommand(string FirstName, string LastName, string Email) : IRequest<DonorDto>;

public class CreateDonorCommandValidator : AbstractValidator<CreateDonorCommand>
{
    public CreateDonorCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
    }
}

public class CreateDonorCommandHandler(IDonorServiceClient donorServiceClient, ILogger<CreateDonorCommandHandler> logger) : IRequestHandler<CreateDonorCommand, DonorDto>
{
    public async Task<DonorDto> Handle(CreateDonorCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating donor {Email}", request.Email);
        var createDto = new CreateDonorDto(request.FirstName, request.LastName, request.Email);
        return await donorServiceClient.CreateDonorAsync(createDto, cancellationToken);
    }
}
