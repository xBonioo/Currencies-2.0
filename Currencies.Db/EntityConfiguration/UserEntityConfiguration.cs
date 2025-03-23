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
            .Property(r => r.Address)
            .HasMaxLength(64);

        builder
            .Property(r => r.IdentityNumber)
            .IsRequired();

        builder
            .Property(r => r.IdNumber)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(r => r.IdExpiryDate)
            .IsRequired();

        builder
            .Property(r => r.IdIssueDate)
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
