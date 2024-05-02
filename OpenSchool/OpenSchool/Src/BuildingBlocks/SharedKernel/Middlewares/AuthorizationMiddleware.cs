using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using SharedKernel.Contracts;
using SharedKernel.Auth;
using SharedKernel.Libraries;
using SharedKernel.Runtime.Exceptions;
using static SharedKernel.Contracts.Enum;

namespace SharedKernel.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthService _authService;
    
    public AuthorizationMiddleware(
        RequestDelegate next,
        IAuthService authService)
    {
        _next = next;
        _authService = authService;
    }
    
    
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint == null)
        {
            await _next(context);
        }
        
        // Lấy thông tin về controller và action từ endpoint.
        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        if (controllerActionDescriptor != null)
        {
            // Lấy attribute AuthorizationRequestAttribute của controller.
            var controllerAttribute = GetAuthorizationAttribute(controllerActionDescriptor.ControllerTypeInfo);
            if (controllerAttribute != null)
            {
                var allowAnonymous = controllerAttribute.Exponents.Contains(ActionExponent.AllowAnonymous);
                if (!allowAnonymous)
                {
                    var hasPermission = _authService.CheckPermission(controllerAttribute.Exponents);
                    if (!hasPermission)
                    {
                        throw new ForbiddenException();
                    }
                }
                
            }
            
            // Lấy attribute AuthorizationRequestAttribute của action.
            var actionAttribute = GetAuthorizationAttribute(controllerActionDescriptor.MethodInfo);
            if (actionAttribute != null)
            {
                var allowAnonymous = actionAttribute.Exponents.Contains(ActionExponent.AllowAnonymous);
                if (!allowAnonymous)
                {
                    var hasPermission = _authService.CheckPermission(actionAttribute.Exponents);
                    if (!hasPermission)
                    {
                        throw new ForbiddenException();
                    }
                }
                
            }
        }
    }
    
    private AuthorizationRequestAttribute? GetAuthorizationAttribute(MemberInfo memberInfo)
    {
        // inherit một cờ (flag) xác định xem liệu phương thức này có nên tìm kiếm các attribute kế thừa từ các lớp cha hay không? (inherit: false là không)
        return (AuthorizationRequestAttribute)memberInfo.GetCustomAttributes(typeof(AuthorizationRequestAttribute), inherit: false).FirstOrDefault();
    }
}

public static class AuthorizationMiddlewareExtension
{
    public static IApplicationBuilder UseCoreAuthorization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<AuthorizationMiddleware>();
    }
}
