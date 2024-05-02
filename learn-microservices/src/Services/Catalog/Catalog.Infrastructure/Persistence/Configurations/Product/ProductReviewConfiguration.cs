using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class ProductReviewConfiguration : BaseEntityConfiguration<ProductReview>
{
    public override void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        base.Configure(builder);

        #region Indexes

        

        #endregion

        #region Columns
        
        builder.Property(pr => pr.Comment)
            .HasMaxLength(255)
            .IsRequired(false);
        
        builder.Property(pr => pr.UserId)
            .IsRequired();
        
        builder.Property(pr => pr.UserInfo)
            .HasMaxLength(512)
            .IsRequired(false);

        #endregion
    }
}