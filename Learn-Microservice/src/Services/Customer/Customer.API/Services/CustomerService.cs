using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Entities.Customer> GetByUserNameAsync(string userName)
        => await _customerRepository.GetByUserName(userName);
    

    public async Task<IEnumerable<Entities.Customer>> GetAllAsync()
        => await _customerRepository.FindAllAsync();
}