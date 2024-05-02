using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure; 
using OnlineShop.Core.Schemas;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.Core.Interfaces
{
    public interface IDataContext: IDisposable
    {
        DbSet<UserSchema> Users { get; set; }
        DbSet<GroupSchema> Groups { get; set; }
        DbSet<PermSchema> Perms { get; set; }
        DbSet<GroupPerm> GroupsPerms { get; set; }
        DbSet<UsersGroups> UsersGroups { get; set; }
        DbSet<ProductSchema> Products { get; set; }
        DbSet<CustomerSchema> Customers { get; set; }
        DbSet<CustomersGroups> CustomerGroups { get; set; }
		DbSet<VoucherSchema> Vouchers { get; set; }
        DbSet<CategorySchema> Categories { get; set; }
        DbSet<BrandSchema> Brands { get; set; }
        DbSet<OrderSchema> Orders { get; set; }
        DbSet<ProductOrderSchema> ProductOrders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void Dispose();
        DatabaseFacade Database { get; }
    }
}
