using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using MediatR;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonorsByRequest;

public sealed class GetDonorsByRequestQueryHandler : IRequestHandler<GetDonorsByRequestQuery, IReadOnlyList<DonorDto>>
{
    private readonly IDonorServiceClient _donorServiceClient;

    public GetDonorsByRequestQueryHandler(IDonorServiceClient donorServiceClient)
    {
        _donorServiceClient = donorServiceClient;
    }

    public async Task<IReadOnlyList<DonorDto>> Handle(GetDonorsByRequestQuery request, CancellationToken cancellationToken)
    {
        return await _donorServiceClient.GetDonorsByRequestAsync(request.RequestId, cancellationToken);
    }
}
