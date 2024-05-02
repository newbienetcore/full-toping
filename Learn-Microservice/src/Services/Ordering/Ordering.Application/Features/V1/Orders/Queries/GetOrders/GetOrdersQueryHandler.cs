using AutoMapper;
using MediatR;
using Serilog;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ApiResult<List<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public GetOrdersQueryHandler(IMapper mapper, IOrderRepository orderRepository, ILogger logger)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    private const string MethodName = "GetOrdersQueryHandler";
    
    public async Task<ApiResult<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {MethodName} - {request.UserName}");
        
        var orderEntities = await _orderRepository.GetOrdersByUserName(request.UserName);
        
        _logger.Information($"END: {MethodName} - {request.UserName}");
        var orderList = _mapper.Map<List<OrderDto>>(orderEntities);

        return new ApiSuccessResult<List<OrderDto>>(orderList);
    }
}