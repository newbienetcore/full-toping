using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SharedKernel.Contracts;

public class BaseEntityBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        return Task.CompletedTask;
    }
}