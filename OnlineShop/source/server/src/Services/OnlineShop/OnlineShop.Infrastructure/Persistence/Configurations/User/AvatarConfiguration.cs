using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShop.Domain.Constants;

namespace OnlineShop.Infrastructure.Persistence.Configurations;

public class AvatarConfiguration : IEntityTypeConfiguration<Avatar>
{
    public void Configure(EntityTypeBuilder<Avatar> builder)
    {
        builder.ToTable(TableName.Avatar);
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.FileName).HasMaxLength(255);
    }
}