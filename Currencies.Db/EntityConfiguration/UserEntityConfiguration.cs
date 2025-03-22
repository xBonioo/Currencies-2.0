using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Currencies.Db.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
             .Property(r => r.FirstName)
             .HasMaxLength(64);

        builder
             .Property(r => r.SecondName)
             .HasMaxLength(64);

        builder
             .Property(r => r.UserName)
             .HasMaxLength(32)
             .IsRequired();

        builder
            .Property(r => r.Adres)
            .HasMaxLength(64);

        builder
            .Property(r => r.IdentityNumber)
            .IsRequired();

        builder
            .Property(r => r.IDNumber)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(r => r.IDExpiryDate)
            .IsRequired();

        builder
            .Property(r => r.IDIssueDate)
            .IsRequired();

        builder
            .HasMany(u => u.UserCurrencyAmounts)
            .WithOne(uc => uc.User)
            .HasForeignKey(uc => uc.UserId);

        builder
            .HasMany(r => r.UserExchangeHistory)
            .WithOne(u => u.User)
            .HasForeignKey(u => u.UserId);
    }
}
