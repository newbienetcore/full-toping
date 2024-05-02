using AutoMapper;
using Contracts.Common.Interfaces;
using MediatR;
using Ordering.Application.Common;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Serilog;
using ApplicationException = Shared.Exceptions.ApplicationException;
using Shared.SeedWork;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger logger)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private const string MethodName = "UpdateOrderCommandHandler";
    
    public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);
        if (order is null) throw new ApplicationException(ErrorCode.OrderNotFound, ErrorCode.OrderNotFound);
        
        _logger.Information($"BEGIN: {MethodName} - Order: {request.Id}");
        
        // order.TotalPrice = request.TotalPrice;
        // order.FirstName = request.FirstName; 
        // order.LastName = request.LastName; 
        // order.EmailAddress = request.EmailAddress;
        // order.ShippingAddress = request.ShippingAddress; 
        // order.InvoiceAddress = request.InvoiceAddress; 

        order = _mapper.Map(request, order);
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information($"Order {request.Id} was successfully updated.");
        
        var result = _mapper.Map<OrderDto>(order);
        
        _logger.Information($"END: {MethodName} - Order: {request.Id}");
        return new ApiSuccessResult<OrderDto>(_mapper.Map<OrderDto>(request));
    }
}