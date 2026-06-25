namespace Donnum.Gateway.Application.Models.Profile;

public record IdentityProfileDto(Guid CredentialId, string Email, string FirstName, string LastName, string Role);

public record CombinedDonorProfileDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string Role,
    object? ClinicalProfile
);
