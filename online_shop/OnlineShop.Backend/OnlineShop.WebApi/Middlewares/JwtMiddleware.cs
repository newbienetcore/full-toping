
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using OnlineShop.Core.Models;
using OnlineShop.WebApi.Routers;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core.Interfaces;
using OnlineShop.Infrastructure.Services;
using OnlineShop.WebApi.Helpers.Exceptions;
using AutoMapper;
using OnlineShop.Core;
using OnlineShop.Core.Interfaces;
using OnlineShop.Infrastructure;
using OnlineShop.UseCases.Permission;

namespace OnlineShop.WebApi.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache cacheService;
        private readonly ICacheService<PermSchema> permCacheService;
        private readonly DataContext dbContext;
        public JwtMiddleware(RequestDelegate next, IMemoryCache _cacheService, DataContext ctx)
        {
            _next = next;
            cacheService = _cacheService;
            dbContext = ctx;
            permCacheService = new CacheService<PermSchema>(cacheService);
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var configuration = context.RequestServices.GetRequiredService<IConfiguration>();
                var secretKey = configuration["Configuration:SecretKey"];
                byte[] key = Encoding.ASCII.GetBytes(secretKey);
                var mapper = context.RequestServices.GetRequiredService<IMapper>();
                IEnumerable<RouterModel> routers = new PublicRouter(dbContext, mapper, key).Get();
                var publicApi = routers.Select(x => x.Path).ToList();
                var apiRequest = context.Request.Path.Value.Replace("/api/", "");
                if (!publicApi.Contains(apiRequest))
                {
                    string accessToken = GetToken(context);
                    if (accessToken != null)
                    {
                        string token = accessToken.Replace("Bearer ", "");
                        int id = HandleJwtToken(key, token);
                        if (id > 0)
                        {
                            var header = context.Request.Path.Value;
                            var httpMethod = context.Request.Method;
                            HandlePermission(header, httpMethod, dbContext, id);
                        }
                    }
                    else
                    {
                        throw new UnauthorizedException("Unauthorized");
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                var exceptionHelper = new ExceptionHelper();
                await exceptionHelper.HandleExceptionAsync(context, ex);
            }
        }

        private string GetToken(HttpContext context)
        {
            string accessToken = context.Request.Cookies["access_token"];
            if (accessToken == null)
            {
                accessToken = context.Request.Headers["Authorization"].FirstOrDefault();
            }
            return accessToken;
        }

        private int HandleJwtToken(byte[] key, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

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
            return id;
        }

        private void HandlePermission(string header, string httpMethod, IDataContext dbContext, int userId)
        {

            bool isRequestApi = header.Contains("/api/");
            if (isRequestApi)
            {
                var apiRequest = header.Replace("/api/", "").Split("/");

                string module = apiRequest.Length > 1 ? apiRequest[1] : apiRequest[0];
                string action = GetAction(apiRequest, httpMethod);

                var permFlow = new VerifyFlow(permCacheService, new PermService(dbContext));
                Response res = permFlow.Execute(module, action, userId);

                if (res.Result == null)
                {
                    throw new ForbiddenException("Forbidden");
                }
            }
        }


        private string GetAction(string[] apiRequest, string httpMethod)
        {
            string action = "";
            if (apiRequest.Length > 1)
            {
                var isNumeric = apiRequest.Length == 2 ? int.TryParse(apiRequest[1], out int n) : false;
                action = isNumeric ? httpMethod : apiRequest[0] + "/" + apiRequest[1];
            }
            else
            {
                action = httpMethod;
            }
            return action;
        }
    }
}
