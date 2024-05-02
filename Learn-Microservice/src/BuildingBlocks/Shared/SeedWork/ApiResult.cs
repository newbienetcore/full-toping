namespace Shared.SeedWork;

public class ApiResult<T>
{
    public ApiResult()
    {
        IsSucceeded = true;
    }

    public ApiResult(bool isSucceeded = true, string message = default!)
    {
        Message = message;
        IsSucceeded = isSucceeded;
    }

    public ApiResult(T data, bool isSucceeded = true, string message = default!)
    {
        Data = data;
        Message = message;
        IsSucceeded = isSucceeded;
    }
    
    public bool IsSucceeded { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}