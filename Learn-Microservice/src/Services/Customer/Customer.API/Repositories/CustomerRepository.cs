using Contracts.Common.Interfaces;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;

namespace Customer.API.Repositories;

public class CustomerRepository : Repository<Entities.Customer, int, ApplicationDbContext>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext dbContext, IUnitOfWork<ApplicationDbContext> unitOfWork) : base(dbContext)
    {
    }
    
    public Task<Entities.Customer> GetByUserName(string userName)
        =>  GetAsync(predicate: c => c.UserName.Equals(userName));
}