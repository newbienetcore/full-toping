namespace Customer.API.Services.Interfaces;

public interface ICustomerService
{
    Task<Entities.Customer> GetByUserNameAsync(string userName);
    Task<IEnumerable<Entities.Customer>> GetAllAsync();
}