using Identity.Domain.Constants;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public class UserConfiguration : EntityAuditConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        // base EntityAuditBase
        base.Configure(builder);

        builder
            .HasKey(e => e.Id);
        
        #region Indexes

        // Non - Index Clustered   
        builder
            .HasIndex(u => u.Id);
        
        // Index Clustered 
        builder
            .HasIndex(u => u.Username)
            .IsUnique()
            .IsClustered();

        #endregion

        #region Property
        
        builder
            .Property(u => u.Username)
            .HasMaxLength(50);

        builder
            .Property(u => u.PasswordHash)
            .HasMaxLength(255);

        builder
            .Property(u => u.Salt)
            .HasMaxLength(255);

        builder
            .Property(u => u.PhoneNumber)
            .HasMaxLength(20)
            .IsUnicode(false);

        builder
            .Property(u => u.Email)
            .HasMaxLength(255)
            .IsUnicode(false);

        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50);

        builder
            .Property(u => u.LastName)
            .HasMaxLength(50);

        builder
            .Property(u => u.Address)
            .HasMaxLength(255);
        
        builder.Property(cd => cd.Gender)
            .HasConversion(
                v => v.ToString(), 
                v => (GenderType)Enum.Parse(typeof(GenderType), v) 
            );
        
        #endregion

        #region Reference property

        builder
            .HasOne(u => u.Avatar)
            .WithOne(a => a.User)
            .HasForeignKey<Avatar>(e => e.OwnerId);
        
        builder
            .HasOne(u => u.UserConfig)
            .WithOne(uc => uc.User)
            .HasForeignKey<UserConfig>(u => u.OwnerId);
        
        builder
            .HasOne(u => u.SecretKey)
            .WithOne(uc => uc.User)
            .HasForeignKey<SecretKey>(u => u.OwnerId);
        
        builder
            .HasOne(u => u.Otp)
            .WithOne(uc => uc.User)
            .HasForeignKey<OTP>(u => u.OwnerId);
        
        builder
            .HasOne(u => u.Mfa)
            .WithOne(uc => uc.User)
            .HasForeignKey<MFA>(u => u.OwnerId);
        
        builder
            .HasMany(u => u.SignInHistories)
            .WithOne(ur => ur.User)
            .HasForeignKey(ur => ur.UserId);
        
        #endregion
    }
}