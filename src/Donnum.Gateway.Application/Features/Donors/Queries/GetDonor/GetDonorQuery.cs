using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonor;

public record GetDonorQuery(Guid Id) : IRequest<DonorDto>;

public class GetDonorQueryValidator : AbstractValidator<GetDonorQuery>
{
    public GetDonorQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class GetDonorQueryHandler(IDonorServiceClient donorServiceClient, ILogger<GetDonorQueryHandler> logger) : IRequestHandler<GetDonorQuery, DonorDto>
{
    public async Task<DonorDto> Handle(GetDonorQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting donor {Id}", request.Id);
        return await donorServiceClient.GetDonorAsync(request.Id, cancellationToken);
    }
}
