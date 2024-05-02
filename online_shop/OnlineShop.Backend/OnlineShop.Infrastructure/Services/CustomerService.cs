using OnlineShop.Core.Schemas;
using OnlineShop.Core.Schemas.Base;
using OnlineShop.Core.Interfaces;
using OnlineShop.Utils;
namespace OnlineShop.Infrastructure.Services
{

    public class CustomerService : ICustomer
    {

        private readonly IDataContext context;

        public CustomerService(IDataContext _ctx)
        {
            context = _ctx;
        }

        public CustomerSchema Authentication(string name, string password)
        {
            List<CustomerSchema> customers = context.Customers.Where(u => !string.IsNullOrEmpty(name) && u.UserName.Equals(name)).ToList();
            string hashedPassword = PwdUtil.ConvertToEncrypt(password); //hashed

            CustomerSchema customer = customers
               .Where(u => !string.IsNullOrEmpty(u.Password))
               .Where(u => u.Password == hashedPassword).FirstOrDefault();

            return customer;
        }

        public void UpdateLoginTime(int customerId)
        {
            CustomerSchema customer = context.Customers.Find(customerId);
            customer.LastLogin = DateTime.UtcNow;
            context.SaveChanges();
        }

        public CustomerSchema SetRefreshToken(string refreshToken, int customerId)
        {
            CustomerSchema customer = context.Customers.Find(customerId);
            customer.HashRefreshToken = refreshToken;
            return customer;
        }

        public bool CheckPermissionAction(int customerId, string endPoint)
        {
            PermSchema perm = (
              from p in context.Perms
              join gp in context.GroupsPerms on p.Id equals gp.PermId
              join g in context.Groups on gp.GroupId equals g.Id
              join cg in context.CustomerGroups on g.Id equals cg.GroupId
              where cg.CustomerId == customerId && p.Action == endPoint
              select p
            ).FirstOrDefault();

            return perm != null;
        }

        public CustomerSchema GetById(int id)
        {
            CustomerSchema customer = context.Customers.FirstOrDefault(x => x.Id == id);
            return customer;
        }

        public CustomerSchema UpdateById(int Id, CustomerSchema updateCustomer)
        {
            CustomerSchema customer = GetById(Id);
            customer.UserName = updateCustomer.UserName;
            customer.Password = updateCustomer.Password;
            customer.Address = updateCustomer.Address;
            customer.Email = updateCustomer.Email;
            customer.Phonenumber = updateCustomer.Phonenumber;
            customer.UpdatedAt = DateTime.Now;
            context.SaveChanges();
            return customer;
        }

        public void UpdateAfterLogin(int customerId, string refreshToken)
		{
			CustomerSchema u = context.Customers.Find(customerId);
			u.HashRefreshToken = refreshToken;
			u.LastLogin = DateTime.UtcNow;
			context.SaveChanges();
		}
	}
}
