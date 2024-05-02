using Contracts.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common;
using Ordering.Application.Common.Interfaces;
using Shared.SeedWork;
using ApplicationException = Shared.Exceptions.ApplicationException;

namespace Ordering.Application.Features.V1.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid orderId) : IRequest<ApiResult<bool>>;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResult<bool>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    
    public async Task<ApiResult<bool>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.orderId, cancellationToken);
        if (order is null) throw new ApplicationException(ErrorCode.OrderNotFound, ErrorCode.OrderNotFound);
        
        _orderRepository.Delete(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ApiResult<bool>();
    }
}