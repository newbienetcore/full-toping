using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Persistence.Configurations;

public class SupplierConfiguration : BaseEntityConfiguration<Supplier>
{
    public override void Configure(EntityTypeBuilder<Supplier> builder)
    {
        base.Configure(builder);
        
        #region Indexes
        
        builder
            .HasIndex(e => e.Alias)
            .IsUnique();
        
        #endregion

        #region Columns
        builder
            .Property(e => e.Name)
            .HasMaxLength(255);
        
        builder
            .Property(e => e.Alias)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder
            .Property(e => e.Description)
            .IsRequired(false);

        builder
            .Property(e => e.Bank)
            .HasMaxLength(50)
            .IsRequired(false);

        builder
            .Property(e => e.AccountNumber)
            .HasMaxLength(50)
            .IsUnicode(false)
            .IsRequired(false);

        builder
            .Property(e => e.BankAddress)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(e => e.AddressOne)
            .HasMaxLength(512)
            .IsRequired(false);

        builder
            .Property(e => e.AddressTwo)
            .HasMaxLength(512)
            .IsRequired(false);

        builder
            .Property(e => e.Phone)
            .HasMaxLength(20);
        
        builder
            .Property(e => e.Email)
            .HasMaxLength(255);

        builder
            .Property(e => e.Fax)
            .HasMaxLength(50)
            .IsRequired(false);

        builder
            .Property(e => e.NationCode)
            .IsRequired(false);

        builder
            .Property(e => e.ProvinceCode)
            .IsUnicode(false)
            .IsRequired(false);
        
        builder
            .Property(e => e.DistrictCode)
            .IsUnicode(false)
            .IsRequired(false);

        builder
            .Property(e => e.Status)
            .HasDefaultValue(true);
        
        builder
            .HasMany(e => e.ProductSuppliers)
            .WithOne(e => e.Supplier)
            .HasForeignKey(e => e.SupplierId)
            .OnDelete(DeleteBehavior.Cascade);

        #endregion
    }
}