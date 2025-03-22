using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Currencies.Db.EntityConfiguration;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .HasKey(a => a.Id);

        builder
            .Property(u => u.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .Property(u => u.IsActive)
            .IsRequired();
    }
}