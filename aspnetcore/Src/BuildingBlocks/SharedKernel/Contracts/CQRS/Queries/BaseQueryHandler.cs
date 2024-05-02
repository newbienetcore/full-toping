using AutoMapper;

namespace SharedKernel.Contracts;

public abstract class BaseQueryHandler
{
    protected readonly IMapper _mapper;
    public BaseQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
}