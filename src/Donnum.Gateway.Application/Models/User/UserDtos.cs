namespace Donnum.Gateway.Application.Models.User;

public record CreateOperatorRequestDto(
    string FirstName,
    string LastName,
    string Password,
    Guid HospitalId,
    string PersonalEmail
);

public record CreateOperatorIdentityRequestDto(
    string FirstName,
    string LastName,
    string PersonalEmail,
    string Password
);

public record CreateOperatorIdentityResponseDto(
    Guid CredentialId,
    string CorporateEmail
);
