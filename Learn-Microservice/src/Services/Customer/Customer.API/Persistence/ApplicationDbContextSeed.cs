using Microsoft.EntityFrameworkCore;

namespace Customer.API.Persistence;

public static class ApplicationDbContextSeed
{
    public static IHost SeedData(this IHost host)
    {

        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.MigrateAsync().GetAwaiter().GetResult();

        CreateCustomer(context, "customer1", "customer", "1", "customer1@local.com").GetAwaiter().GetResult();
        CreateCustomer(context, "customer2", "customer", "2", "customer2@local.com").GetAwaiter().GetResult();
        return host;
    }

    private static async Task CreateCustomer(
        ApplicationDbContext context,
        string userName,
        string firstName,
        string lastName,
        string emailAddress)
    {
        var customer = await context.Customers
            .SingleOrDefaultAsync(c => c.UserName.Equals(userName)
                                       || c.EmailAddress.Equals(emailAddress));

        if (customer is null)
        {
            var newCustomer = new Entities.Customer()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress
            };

            await context.Customers.AddAsync(newCustomer);
            await context.SaveChangesAsync();
        }
    }
}