using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductPricingConfiguration : EntityConfiguration<ProductPricing>
{
    public override void Configure(EntityTypeBuilder<ProductPricing> builder)
    {
        base.Configure(builder);
        
        
    }
}