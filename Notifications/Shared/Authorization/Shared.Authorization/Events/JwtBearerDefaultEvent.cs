using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Shared.Authorization.Events;

public class JwtBearerDefaultEvent : JwtBearerEvents
{
    public override Task TokenValidated(TokenValidatedContext context)
    {
        var result = context.SecurityToken as JwtSecurityToken;
        var claims = new List<Claim>();

        foreach (var item in result.Payload)
        {
            if (!context.Principal.HasClaim(x => x.Type.Equals(item.Key)))
            {
                claims.Add(new Claim(item.Key, item.Value.ToString()));
            }
        }

        var claimsIdentity = new ClaimsIdentity(claims);
        
        context.Principal.AddIdentity(claimsIdentity);
        
        return base.TokenValidated(context);
    }
}