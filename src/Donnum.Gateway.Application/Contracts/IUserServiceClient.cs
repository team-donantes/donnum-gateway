using Donnum.Gateway.Application.Models.Profile;

namespace Donnum.Gateway.Application.Contracts;

public interface IUserServiceClient
{
    Task<IdentityProfileDto?> GetMeAsync(string token, CancellationToken cancellationToken = default);
}
