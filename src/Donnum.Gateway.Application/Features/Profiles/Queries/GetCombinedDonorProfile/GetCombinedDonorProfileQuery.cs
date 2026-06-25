using Donnum.Gateway.Application.Contracts;
using Donnum.Gateway.Application.Models.Profile;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Donnum.Gateway.Application.Features.Profiles.Queries.GetCombinedDonorProfile;

public record GetCombinedDonorProfileQuery(Guid UserId, string AuthToken) : IRequest<CombinedDonorProfileDto?>;

public class GetCombinedDonorProfileQueryHandler(
    IUserServiceClient userServiceClient, 
    IDonorServiceClient donorServiceClient,
    ILogger<GetCombinedDonorProfileQueryHandler> logger) : IRequestHandler<GetCombinedDonorProfileQuery, CombinedDonorProfileDto?>
{
    public async Task<CombinedDonorProfileDto?> Handle(GetCombinedDonorProfileQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching combined profile for user {UserId}", request.UserId);

        var meData = await userServiceClient.GetMeAsync(request.AuthToken, cancellationToken);
        if (meData == null)
        {
            logger.LogWarning("Identity profile not found for user {UserId}", request.UserId);
            return null;
        }

        object? clinicalProfile = null;
        try
        {
            var donorData = await donorServiceClient.GetDonorAsync(request.UserId, cancellationToken);
            if (donorData != null)
            {
                clinicalProfile = new
                {
                    donorData.BloodGroup,
                    donorData.RhFactor,
                    donorData.Street,
                    donorData.City,
                    donorData.Province,
                    donorData.Points,
                    donorData.Reliability
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, "Donor profile not found for {UserId}. Returning identity only.", request.UserId);
        }

        return new CombinedDonorProfileDto(
            Id: request.UserId,
            Email: meData.Email,
            FirstName: meData.FirstName,
            LastName: meData.LastName,
            Role: meData.Role,
            ClinicalProfile: clinicalProfile
        );
    }
}
