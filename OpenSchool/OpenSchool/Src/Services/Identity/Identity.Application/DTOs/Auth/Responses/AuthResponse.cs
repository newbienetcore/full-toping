namespace Identity.Application.DTOs.Auth;

public class AuthResponse
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public bool IsVerifyCode { get; set; } = false;

}