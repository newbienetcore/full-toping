using AutoMapper;

namespace SharedKernel.Application
{
    public abstract class BaseQueryHandler
    {
        protected readonly IMapper _mapper;

        public BaseQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
