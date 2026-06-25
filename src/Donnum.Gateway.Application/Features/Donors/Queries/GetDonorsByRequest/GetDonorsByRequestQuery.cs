using Donnum.Gateway.Application.Models.Donor;
using MediatR;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonorsByRequest;

public record GetDonorsByRequestQuery(Guid RequestId) : IRequest<IReadOnlyList<DonorDto>>;
