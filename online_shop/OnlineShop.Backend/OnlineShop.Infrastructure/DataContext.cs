using Microsoft.EntityFrameworkCore;
using OnlineShop.Core.Interfaces;
using OnlineShop.Core.Schemas;
using OnlineShop.Core.Schemas.Base;

namespace OnlineShop.Infrastructure
{
	public partial class DataContext : DbContext, IDataContext
	{
		public DataContext()
		{
		}

		public DataContext(DbContextOptions<DataContext> options)
			: base(options)
		{
		}

		public virtual DbSet<UserSchema> Users { get; set; }
		public virtual DbSet<GroupSchema> Groups { get; set; }
		public virtual DbSet<PermSchema> Perms { get; set; }
		public virtual DbSet<GroupPerm> GroupsPerms { get; set; }
		public virtual DbSet<UsersGroups> UsersGroups { get; set; }
		public virtual DbSet<ProductSchema> Products { get; set; }
		public virtual DbSet<CategorySchema> Categories { get; set; }
		public virtual DbSet<BrandSchema> Brands { get; set; }
		public virtual DbSet<CustomerSchema> Customers { get; set; }
		public virtual DbSet<CustomersGroups> CustomerGroups { get; set; }
		public virtual DbSet<OrderSchema> Orders { get; set; }
		public virtual DbSet<VoucherSchema> Vouchers { get; set; }
		public virtual DbSet<ShipmentSchema> Shipments { get; set; }
		public virtual DbSet<ProductOrderSchema> ProductOrders { get; set; }
		public virtual DbSet<AddressSchema> AddressSchemas { get; set; }
		public virtual DbSet<ProductColor> ProductColors { get; set; }
		public virtual DbSet<ProductSize> ProductSizes { get; set; }
		public virtual DbSet<ColorSchema> Colors { get; set; }
		public virtual DbSet<SizeSchema> Sizes { get; set; }
		public virtual DbSet<ImageSchema> Images { get; set; }

		protected override void OnConfiguring(
			DbContextOptionsBuilder optionsBuilder
		)
		{
			base.OnConfiguring(optionsBuilder);

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
