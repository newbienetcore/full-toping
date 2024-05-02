using OnlineShop.Core;
using OnlineShop.Core.Schemas;
using OnlineShop.Core.Interfaces;
using OnlineShop.Utils;
using OnlineShop.Application.UseCases.Auth;

namespace OnlineShop.UseCases.Customer.Crud
{
    public class CrudCustomerFlow
    {
		public readonly DateTime currentTime = DateTime.Now;
		private readonly IDataContext dbContext;
        private readonly ICustomer _customer;
        public CrudCustomerFlow(IDataContext ctx,ICustomer customer)
        {
            dbContext = ctx;
            _customer = customer;
        }
		public Response Login(string username, string password)
		{
			var customer = _customer.Authentication(username, password);

			if (customer == null)
			{
				return new Response(Message.ERROR, new { });
			}
            _customer.UpdateLoginTime(customer.Id);
			return new Response(Message.SUCCESS, customer);
		}

		public Response Create(CustomerSchema customer)
        {
			customer.Password = PwdUtil.ConvertToEncrypt(customer.Password);
			customer.CreatedAt = currentTime;
			customer.UpdatedAt = currentTime;
			dbContext.Customers.Add(customer);
            dbContext.SaveChanges();
            return new Response(Message.SUCCESS, customer);
        }

        public Response Update(CustomerSchema user)
        {
            var result = dbContext.Customers.Update(user);
            return new Response(Message.SUCCESS, result);

        }
        public Response UpdateCustomer(int Id, CustomerSchema customer)
        {
            var result = _customer.UpdateById(Id, customer);
            return new Response(Message.SUCCESS, result);
        }

        public Response List()
        {
            var customers = dbContext.Customers.ToList();
            return new Response(Message.SUCCESS, customers);
        }

		public Response Get(int id)
		{
			var customer = dbContext.Customers.Find(id);
			return new Response(Message.SUCCESS, customer);
		}
		public Response GetUserIdByUserName(string UserName)
		{
			CustomerSchema customer = dbContext.Customers.FirstOrDefault(u => u.UserName.Equals(UserName));
            if(customer == null)
            {
				return new Response(Message.ERROR, new { });
			}
			return new Response(Message.SUCCESS, customer.Id);
		}

		public Response Delete(int id)
        {
            var customer = dbContext.Customers.Find(id);
            var result = dbContext.Customers.Remove(customer);
            return new Response(Message.SUCCESS, result);
        }

        //public Response Deletes(int[] ids)
        //{
        //    var selectedItems = dbContext.Users.Where(item => ids.Contains((int)item.GetType().GetProperty("Id").GetValue(item))).ToList();
        //    dbContext.Users.RemoveRange(selectedItems);
        //    dbContext.SaveChanges();
        //    return new Response(Message.SUCCESS, ids);
        //}
    }
}
