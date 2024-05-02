using AutoMapper;
using Contracts.Common.Interfaces;
using Contracts.Services;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Serilog;
using Shared.SeedWork;
using Shared.Services.Email;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResult<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly ISmtpEmailService _smtpEmailService;

    public CreateOrderCommandHandler(IOrderRepository orderRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger logger,
        ISmtpEmailService smtpEmailService)
    {
        _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _smtpEmailService = smtpEmailService ?? throw new ArgumentNullException(nameof(smtpEmailService));
    }

    private const string MethodName = "CreateOrderCommandHandler";

    public async Task<ApiResult<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.Information($"BEGIN: {MethodName} - Username: {request.UserName}");
        var order = _mapper.Map<Order>(request);

        await _orderRepository.InsertAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _logger.Information($"Order {order.Id} is successfully created.");

        _logger.Information($"END: {MethodName} - Username: {request.UserName}");
        return new ApiSuccessResult<Guid>(order.Id);
    }
    
}