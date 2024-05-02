using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using SharedKernel.Contracts;
using SharedKernel.Domain;
using SharedKernel.Libraries;
using SharedKernel.MySQL;
using UAParser;

namespace SharedKernel.Middlewares;

public class RequestMiddleware
{
    private readonly RequestDelegate _next;
    
    public RequestMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        var httpRequest = context.Request;
        var openRequest = new OpenHttpRequest(httpRequest.Method, httpRequest.Scheme, httpRequest.Host, httpRequest.Path, httpRequest.QueryString, httpRequest.Headers, AuthUtility.TryGetIP(httpRequest));
        
        context.Request.Headers.Append(HeaderNamesExtension.RequestId, requestId);
        _ = Task.Run(async () =>
        {
            try
            {
                using (var dbConnection = new DbConnection("CentralizedRequestsDb"))
                {
                    var param = GetParameter(openRequest, requestId);
                    await dbConnection.ExecuteAsync(param.Item1, param.Item2, autoCommit: true);
                }
            }
            catch (Exception ex)
            {
                
            }
        });
        await _next(context);
    }
    
    private (string, RequestInformation) GetParameter(OpenHttpRequest request, string requestId)
    {
        var ua = request.Headers[HeaderNames.UserAgent].ToString();
        var c = Parser.GetDefault().Parse(ua);
        var method = request.Method;
        var origin = request.Headers[HeaderNames.Origin];
        var device = c.Device.Family;
        var apiUrl = $"{request.Scheme}://{request.Host}{request.Path.Value}{request.QueryString.Value}";
        var ip = request.Ip;
        var browser = c.UA.Family + (!string.IsNullOrEmpty(c.UA.Major) ? $" {c.UA.Major}.{c.UA.Minor}" : "");
        var os = c.OS.Family + (!string.IsNullOrEmpty(c.OS.Major) ? $" {c.OS.Major}" : "") + (!string.IsNullOrEmpty(c.OS.Minor) ? $".{c.OS.Minor}" : "");

        var properties = typeof(RequestInformation).GetProperties().Where(p => p.GetIndexParameters().Length == 0);
        var columns = string.Join(", ", properties.Select(p => $"`{p.Name}`"));
        var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));
        var param = new RequestInformation(requestId, ip, method, apiUrl, browser, os, device, ua, origin);
        var cmd = $"INSERT INTO {param.GetTableName()}( {columns} ) VALUES ( {parameters} );";

        return (cmd, param);
    }
}

public static class RequestMiddlewareExtension
{
    public static IApplicationBuilder UseCoreRequest(this IApplicationBuilder app)
    {
        return app.UseMiddleware<RequestMiddleware>();
    }
}