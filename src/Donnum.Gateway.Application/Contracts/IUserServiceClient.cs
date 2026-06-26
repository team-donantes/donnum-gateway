using Donnum.Gateway.Application.Models.Profile;
using Donnum.Gateway.Application.Models.User;

namespace Donnum.Gateway.Application.Contracts;

public interface IUserServiceClient
{
    Task<IdentityProfileDto?> GetMeAsync(string token, CancellationToken cancellationToken = default);
    Task<CreateOperatorIdentityResponseDto> CreateOperatorIdentityAsync(CreateOperatorIdentityRequestDto request, CancellationToken cancellationToken = default);
}
