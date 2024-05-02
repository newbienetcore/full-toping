using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;



public class ProductSupplierConfiguration : EntityConfiguration<ProductSupplier>
{
    public override void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
        base.Configure(builder);
        
        #region Indexes

        

        #endregion

        #region Columns

        builder
            .HasKey(sc => new { sc.ProductId, sc.SupplierId });
        
        #endregion
    }
}