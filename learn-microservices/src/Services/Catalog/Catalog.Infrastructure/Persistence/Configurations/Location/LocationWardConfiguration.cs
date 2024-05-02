using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class LocationWardConfiguration : IEntityTypeConfiguration<LocationWard>
{
    public void Configure(EntityTypeBuilder<LocationWard> builder)
    {
        #region Indexes

        builder
            .HasIndex(e => e.Code)
            .IsUnique();

        #endregion

        #region Columns
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        
        builder
            .HasKey(e => e.Id);
        
        #endregion
    }
}