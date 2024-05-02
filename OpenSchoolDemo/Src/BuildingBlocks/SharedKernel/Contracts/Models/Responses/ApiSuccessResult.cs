using System.Net;

namespace SharedKernel.Contracts;

public class ApiSuccessResult : ApiResult
{
    public ApiSuccessResult()
    {
       
    }
}

public class ApiSuccessResult<T> : ApiResult
{
    public T Data { get; set; }

    public ApiSuccessResult()
    {
        Data = default;
    }
    

    public ApiSuccessResult(T data)
    {
        Data = data;
    }
}