using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Currencies.Db.EntityConfiguration;

public class CurrencyEntityConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(r => r.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(r => r.Symbol)
            .HasMaxLength(3)
            .IsRequired();

        builder
            .Property(r => r.Description)
            .HasMaxLength(256);

        builder
            .Property(u => u.IsActive)
            .IsRequired();
    }
}
