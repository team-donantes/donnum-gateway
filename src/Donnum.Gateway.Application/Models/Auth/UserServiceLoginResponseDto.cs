namespace Donnum.Gateway.Application.Models.Auth;

public class UserServiceLoginResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
