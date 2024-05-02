using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : Repository<Order,Guid,ApplicationDbContext>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext dbContext, IUnitOfWork unitOfWork) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        => await FindAllAsync(predicate: o => o.UserName.Equals(userName));
    
}