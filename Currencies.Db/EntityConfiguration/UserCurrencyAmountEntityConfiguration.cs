using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Currencies.Db.EntityConfiguration;

public class UserCurrencyAmountEntityConfiguration : IEntityTypeConfiguration<UserCurrencyAmount>
{
    public void Configure(EntityTypeBuilder<UserCurrencyAmount> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .HasOne(uc => uc.Currency)
            .WithMany()
            .HasForeignKey(uc => uc.CurrencyId);

        builder
            .HasOne(u => u.User)
            .WithMany(u => u.UserCurrencyAmounts)
            .HasForeignKey(u => u.UserId);

        builder
            .Property(u => u.Amount)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder
            .Property(u => u.IsActive)
            .IsRequired();
    }
}