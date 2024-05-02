using System.Net;
using Newtonsoft.Json;

namespace Notifications.Application.Common.Models.Responses;

public class ApiResponseBase
{
    [JsonProperty("statusCode")]
    public HttpStatusCode StatusCode { get; set; }
    
    [JsonProperty("errorKey")]
    public string ErrorKey { get; set; }
    
    [JsonProperty("error")]
    public Dictionary<string, List<string>> Error { get; set; }
    
    [JsonProperty("status")]
    public bool Status { get => string.IsNullOrEmpty(ErrorKey); }
    
    public ApiResponseBase()
    {
        StatusCode = HttpStatusCode.OK;
    }

    public ApiResponseBase(HttpStatusCode httpStatusCode, string errorMessage)
    {
        ErrorKey = errorMessage;
        StatusCode = httpStatusCode;
    }
}