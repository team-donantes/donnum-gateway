namespace Donnum.Gateway.Application.Models.Donor;

public record DonorDto(Guid Id, string FirstName, string LastName, string Email);
public record CreateDonorDto(string FirstName, string LastName, string Email);
public record UpdateDonorDto(string FirstName, string LastName);
