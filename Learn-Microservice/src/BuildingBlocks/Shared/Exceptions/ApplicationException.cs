namespace Shared.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException()
    {
    }

    public ApplicationException(string errorKey, string message) : base(message)
    {
        ErrorKey = errorKey;
    }

    public ApplicationException(string message) : base(message)
    {
    }

    public string ErrorKey { get; set; }
}