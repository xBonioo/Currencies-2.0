using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Currencies.Db.EntityConfiguration;

public class ExchangeRateEntityConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .HasOne(e => e.FromCurrency)
            .WithMany()
            .HasForeignKey(e => e.FromCurrencyId);

        builder
            .HasOne(e => e.ToCurrency)
            .WithMany()
            .HasForeignKey(e => e.ToCurrencyId);

        builder
            .Property(r => r.Rate)
            .HasColumnType("decimal(18, 6)")
            .IsRequired();

        builder
            .Property(r => r.Direction)
            .IsRequired();

        builder
            .Property(r => r.IsActive)
            .IsRequired();

    }
}
