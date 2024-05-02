using Contracts.Common.Interfaces;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces;

public interface ICustomerRepository : IRepository<Entities.Customer, int, ApplicationDbContext>
{
    Task<Entities.Customer> GetByUserName(string userName);
}