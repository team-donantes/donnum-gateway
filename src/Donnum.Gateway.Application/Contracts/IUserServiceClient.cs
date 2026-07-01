using Donnum.Gateway.Application.Models.Profile;
using Donnum.Gateway.Application.Models.User;
using Donnum.Gateway.Application.Models.Auth;

namespace Donnum.Gateway.Application.Contracts;

public interface IUserServiceClient
{
    Task<IdentityProfileDto?> GetMeAsync(string token, CancellationToken cancellationToken = default);
    Task<CreateOperatorIdentityResponseDto> CreateOperatorIdentityAsync(string token, CreateOperatorIdentityRequestDto request, CancellationToken cancellationToken = default);
    Task<UserServiceLoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserBatchDataDto>> GetUsersDataBatchAsync(IEnumerable<Guid> userIds, CancellationToken cancellationToken = default);
}
