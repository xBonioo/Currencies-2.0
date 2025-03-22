using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Currencies.Db.EntityConfiguration;

public class UserExchangeHistoryEntityConfiguration : IEntityTypeConfiguration<UserExchangeHistory>
{
    public void Configure(EntityTypeBuilder<UserExchangeHistory> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .HasOne(u => u.User)
            .WithMany(u => u.UserExchangeHistory)
            .HasForeignKey(u => u.UserID);

        builder
           .HasOne(u => u.Rate)
           .WithMany()
           .HasForeignKey(u => u.RateID)
           .IsRequired(false);

        builder
            .Property(u => u.Amount)
            .HasColumnType("decimal(18, 2)")
            .IsRequired();

        builder
          .HasOne(u => u.Account)
          .WithMany()
          .HasForeignKey(u => u.AccountID);

        builder
          .Property(u => u.PaymentStatus)
          .IsRequired();

    }
}