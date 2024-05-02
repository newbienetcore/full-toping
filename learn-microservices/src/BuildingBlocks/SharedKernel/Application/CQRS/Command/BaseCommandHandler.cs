using AutoMapper;
using SharedKernel.Domain;

namespace SharedKernel.Application
{
    public abstract class BaseCommandHandler
    {
        protected readonly IServiceProvider _provider;
        
        public BaseCommandHandler(IServiceProvider provider)
        {
            _provider = provider;
        }
    }
}
