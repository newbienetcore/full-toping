using Catalog.Domain.Entities;
using Catalog.Domain.Enum;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enum = System.Enum;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductDiscountConfiguration : BaseEntityConfiguration<ProductDiscount>
{
    public override void Configure(EntityTypeBuilder<ProductDiscount> builder)
    {
        base.Configure(builder);
        
        builder.Property(cd => cd.DiscountUnit)
            .HasConversion(
                v => v.ToString(),   
                v => (DiscountUnit)Enum.Parse(typeof(DiscountUnit), v)  
            );
        
        builder.Property(cd => cd.CouponCode)
            .HasMaxLength(255)
            .IsUnicode(false);
    }
}