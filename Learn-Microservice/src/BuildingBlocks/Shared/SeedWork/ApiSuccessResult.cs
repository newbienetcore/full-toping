namespace Shared.SeedWork;

public class ApiSuccessResult<T> : ApiResult<T>
{
    public ApiSuccessResult(T data) : base(data, true,"Success")
    {
        
    }

    public ApiSuccessResult(T data, string message) : base(data, true, message)
    {
        
    }
}