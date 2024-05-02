namespace Identity.Application.DTOs.Auth;

public class SignUpDto
{
    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string ConfirmPassword { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public string Address { get; init; } = string.Empty;

    public DateTime DateOfBirth { get; init; }

    public GenderType Gender { get; init; }
}