using System.Net;

namespace Notifications.Application.Common.Models.Responses;

public class ApiResponse : ApiResponseBase
{
    public ApiResponse()
    {
        StatusCode = HttpStatusCode.OK;
    }
}

public class ApiResponse<T> : ApiResponseBase
{
    public T Data { get; set; }

    public ApiResponse()
    {
        Data = default;
    }
    

    public ApiResponse(T data)
    {
        Data = data;
        StatusCode = HttpStatusCode.OK;
    }
}