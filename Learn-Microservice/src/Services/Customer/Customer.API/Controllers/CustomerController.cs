using Customer.API.Constants;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.SeedWork;
using ApplicationException = Shared.Exceptions.ApplicationException;

namespace Customer.API.Controllers;

public static class CustomerController
{
    public static void MapCustomersAPI(this WebApplication app)
    {
        app.MapGet("/", () => "Welcome to Customer API");
    
        // GET ALL
        app.MapGet("/api/customers", 
            async (ICustomerService CustomerService) => 
                new ApiResponse<IEnumerable<Customer.API.Entities.Customer>>(await CustomerService.GetAllAsync()));

        // GET BY USER NAME
        app.MapGet("/api/customers/{username}",
            async (string userName, ICustomerService CustomerService) =>
            {
                var customer = await CustomerService.GetByUserNameAsync(userName);
                
                if(customer == null) throw new ApplicationException(ErrorCode.CustomerNotFound, ErrorCode.CustomerNotFound);

                return new ApiResponse<Customer.API.Entities.Customer>(customer);
            });
        
        // CREATE CUSTOMER
        app.MapPost("/api/customers", async (Customer.API.Entities.Customer customer, ICustomerRepository customerRepository) =>
        {
            var duplicate = await customerRepository.GetAsync(
                predicate: c => c.UserName.Equals(customer.UserName) || c.EmailAddress.Equals(customer.EmailAddress));

            if (duplicate is not null)
                throw new ApplicationException(ErrorCode.CustomerUserNameOrEmailAddressIsExist, ErrorCode.CustomerUserNameOrEmailAddressIsExist);

            await customerRepository.InsertAsync(customer);
            await customerRepository.SaveChangesAsync();
            
            return new ApiResponse();
        });

        // UPDATE CUSTOMER
        app.MapPut("/api/customers/{id:int}", async (int id, Customer.API.Entities.Customer customer, ICustomerRepository customerRepository) =>
        {
            var cus = await customerRepository.GetAsync(predicate: c => c.Id == id);
            if(cus is null) throw new ApplicationException(ErrorCode.CustomerNotFound, ErrorCode.CustomerNotFound);
            
            var duplicate = await customerRepository.GetAsync(
                predicate: c => (c.UserName.Equals(customer.UserName) || c.EmailAddress.Equals(customer.EmailAddress)) && c.Id != id);

            if (duplicate is not null)
                throw new ApplicationException(ErrorCode.CustomerUserNameOrEmailAddressIsExist,
                    ErrorCode.CustomerUserNameOrEmailAddressIsExist);

            cus.UserName = customer.UserName;
            cus.FirstName = customer.FirstName;
            cus.LastName = customer.LastName;
            cus.EmailAddress = customer.EmailAddress;
            
            customerRepository.Update(cus);
            await customerRepository.SaveChangesAsync();
            
            return new ApiResponse();
        });

        // DELETE CUSTOMER
        app.MapDelete("/api/customers/{id:int}",  async (int id, ICustomerRepository customerRepository) =>
        {
            var cus = await customerRepository.GetAsync(predicate: c => c.Id == id);
            if(cus is null) throw new ApplicationException(ErrorCode.CustomerNotFound, ErrorCode.CustomerNotFound);
            
            customerRepository.Delete(cus);
            await customerRepository.SaveChangesAsync();
            
            return new ApiResponse();
        });
    }
}