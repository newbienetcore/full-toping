using FluentValidation.Results;

namespace Ordering.Application.Common.Exceptions;

public class ValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }
    
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failuresGroup => failuresGroup.Key, failuresGroup => failuresGroup.ToArray());
    }
}