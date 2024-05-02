using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using OnlineShop.Application.Helpers; 
using OnlineShop.Application.Helpers.Exceptions;

namespace OnlineShop.Application.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var cookieToken = context.Request.Cookies["access_token"];
                var tokenHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (cookieToken != null || (tokenHeader != null && tokenHeader != "null"))
                {
                    string token = cookieToken == null ? tokenHeader.Replace("Bearer ", "") : cookieToken;
                    AttachUserToContext(context, token);
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                var middlewareHelper = new MiddlewareHelper();
                await middlewareHelper.HandleExceptionAsync(context, ex);
            }
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            if (context.Request.Path.Value != null && (context.Request.Path.Value.ToLower() == "/api/auth/refresh" || context.Request.Path.Value.ToLower() == "/api/users/get-current-user" || context.Request.Path.Value.ToLower() == "/api/auth/login"))
            {
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            string secretKey = _configuration["Configuration:SecretKey"];
            byte[] key = Encoding.ASCII.GetBytes(secretKey);
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userCredentialString = jwtToken.Claims.First(x => x.Type == "id").Value;
            int id = Int32.Parse(userCredentialString);
            if (id == null)
            {
                throw new UnauthorizedException("Unauthorized");
            }
            // do not Check permission
        }
    }
}
