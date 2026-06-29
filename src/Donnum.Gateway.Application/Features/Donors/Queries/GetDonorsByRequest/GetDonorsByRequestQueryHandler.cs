using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Donor;
using MediatR;

namespace Donnum.Gateway.Application.Features.Donors.Queries.GetDonorsByRequest;

public sealed class GetDonorsByRequestQueryHandler : IRequestHandler<GetDonorsByRequestQuery, IReadOnlyList<DonorDto>>
{
    private readonly IDonorServiceClient _donorServiceClient;
    private readonly IUserServiceClient _userServiceClient;

    public GetDonorsByRequestQueryHandler(
        IDonorServiceClient donorServiceClient,
        IUserServiceClient userServiceClient)
    {
        _donorServiceClient = donorServiceClient;
        _userServiceClient = userServiceClient;
    }

    public async Task<IReadOnlyList<DonorDto>> Handle(GetDonorsByRequestQuery request, CancellationToken cancellationToken)
    {
        var donors = await _donorServiceClient.GetDonorsByRequestAsync(request.RequestId, cancellationToken);
        
        if (donors == null || !donors.Any())
        {
            return donors ?? Array.Empty<DonorDto>();
        }

        var donorIds = donors.Select(d => d.Id).ToList();
        var usersData = await _userServiceClient.GetUsersDataBatchAsync(donorIds, cancellationToken);
        var usersDict = usersData.ToDictionary(u => u.Id);

        var enrichedDonors = donors.Select(d => 
        {
            if (usersDict.TryGetValue(d.Id, out var userData))
            {
                return d with { FirstName = userData.FirstName, LastName = userData.LastName };
            }
            return d;
        }).ToList();

        return enrichedDonors;
    }
}
