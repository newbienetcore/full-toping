namespace SharedKernel.Runtime;

public class ExceptionHandler : IExceptionHandler
{
    public Task PutToDatabaseAsync(Exception ex)
    {
        throw new NotImplementedException();
    }
}