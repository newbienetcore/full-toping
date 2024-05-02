using System.Net;

namespace Shared.SeedWork;

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