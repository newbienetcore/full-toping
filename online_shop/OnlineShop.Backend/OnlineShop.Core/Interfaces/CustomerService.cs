using OnlineShop.Core.Schemas;
namespace OnlineShop.Core.Interfaces
{
    public interface ICustomer
    {
        bool CheckPermissionAction(int customer, string endPoint);
		void UpdateLoginTime(int customerId);
        CustomerSchema UpdateById(int Id, CustomerSchema updateCustomer);

        CustomerSchema Authentication(string name, string password);
		void UpdateAfterLogin(int customerId, string refreshToken);

	}
}
