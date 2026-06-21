using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Commands.UpdateDonor;

public record UpdateDonorCommand(Guid Id, string FirstName, string LastName) : IRequest<DonorDto>;

public class UpdateDonorCommandValidator : AbstractValidator<UpdateDonorCommand>
{
    public UpdateDonorCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
    }
}

public class UpdateDonorCommandHandler(IDonorServiceClient donorServiceClient, ILogger<UpdateDonorCommandHandler> logger) : IRequestHandler<UpdateDonorCommand, DonorDto>
{
    public async Task<DonorDto> Handle(UpdateDonorCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating donor {Id}", request.Id);
        var updateDto = new UpdateDonorDto(request.FirstName, request.LastName);
        return await donorServiceClient.UpdateDonorAsync(request.Id, updateDto, cancellationToken);
    }
}
