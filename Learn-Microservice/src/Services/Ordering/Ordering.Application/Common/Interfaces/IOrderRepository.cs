using Contracts.Common.Interfaces;
using Ordering.Domain.Entities;

namespace Ordering.Application.Common.Interfaces;

public interface IOrderRepository : IRepository<Order, Guid>
{
    Task<IEnumerable<Order>> GetOrdersByUserName(string userName);
}