using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notifications.Application.Common.Interfaces;
using Notifications.Domain.Configurations;
using Notifications.Domain.Entities;
using Notifications.Infrastructure.Identity;

namespace Notifications.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IIdentityService _identityService;
    
    public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings, IIdentityService identityService)
    {
        _jwtSettings = jwtSettings.Value ?? throw new ArgumentNullException(nameof(jwtSettings));
        _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
    }
    
    public async Task<string> Generate(User user)
    {
        var roles = await _identityService.GetRolesAsync(user);
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiredMinute),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }
}