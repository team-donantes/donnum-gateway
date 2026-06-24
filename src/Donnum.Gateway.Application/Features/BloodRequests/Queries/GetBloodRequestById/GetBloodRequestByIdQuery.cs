using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.BloodRequest;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.BloodRequests.Queries.GetBloodRequestById;

public record GetBloodRequestByIdQuery(Guid Id) : IRequest<BloodRequestDto>;

public class GetBloodRequestByIdQueryValidator : AbstractValidator<GetBloodRequestByIdQuery>
{
    public GetBloodRequestByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}

public class GetBloodRequestByIdQueryHandler(IBloodRequestServiceClient bloodRequestServiceClient, ILogger<GetBloodRequestByIdQueryHandler> logger) : IRequestHandler<GetBloodRequestByIdQuery, BloodRequestDto>
{
    public async Task<BloodRequestDto> Handle(GetBloodRequestByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting blood request {Id}", request.Id);
        return await bloodRequestServiceClient.GetBloodRequestByIdAsync(request.Id, cancellationToken);
    }
}
