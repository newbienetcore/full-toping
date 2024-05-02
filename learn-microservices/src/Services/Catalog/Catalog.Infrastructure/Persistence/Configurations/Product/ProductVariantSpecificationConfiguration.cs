using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductVariantSpecificationConfiguration : EntityConfiguration<ProductVariantSpecification>
{
    public override void Configure(EntityTypeBuilder<ProductVariantSpecification> builder)
    {
        base.Configure(builder);
        
        builder
            .HasKey(e => e.Id);
        
        
    }
}