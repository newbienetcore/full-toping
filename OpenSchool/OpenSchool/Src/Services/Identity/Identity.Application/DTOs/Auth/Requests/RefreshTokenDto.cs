namespace Identity.Application.DTOs.Auth;

public class RefreshTokenDto
{
    public Guid UserId { get; set; }

    public string RefreshToken { get; set; }
}