using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Infrastructure;
using Currencies.Db.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Currencies.Db;

public class TableContext : IdentityDbContext<
        ApplicationUser, IdentityRole, string,
        IdentityUserClaim<string>,
        IdentityUserRole<string>,
        IdentityUserLogin<string>,
        IdentityRoleClaim<string>,
        TokenUser>
{
    public virtual DbSet<Currency> Currencies => Set<Currency>();
    public virtual DbSet<ExchangeRate> ExchangeRate => Set<ExchangeRate>();
    public virtual DbSet<UserExchangeHistory> UserExchangeHistories => Set<UserExchangeHistory>();
    public virtual DbSet<UserCurrencyAmount> UserCurrencyAmounts => Set<UserCurrencyAmount>();
    public virtual DbSet<Role> Roles => Set<Role>();
    
    public TableContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.StateChanged += Timestamps;
        ChangeTracker.Tracked += Timestamps;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TableContext).Assembly);

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                                     .SelectMany(t => t.GetForeignKeys())
                                     .Where(fk => fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }

        base.OnModelCreating(modelBuilder);

        TestDataSeed(modelBuilder);
    }

    private void Timestamps(object? sender, EntityEntryEventArgs e)
    {
        if (sender is null)
        {
            return;
        }
        if (e.Entry.Entity is ICreatable createdEntity &&
            e.Entry.State == EntityState.Added)
        {
            createdEntity.CreatedOn = DateTime.UtcNow;
        }
        else if (e.Entry.Entity is IModifable modifiedEntity &&
        e.Entry.State == EntityState.Modified)
        {
            modifiedEntity.ModifiedOn = DateTime.UtcNow;
        }
    }

    private void TestDataSeed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin"
                },
                new Role
                {
                    Id = 2,
                    Name = "Użytkownik"
                });


        modelBuilder.Entity<Currency>().HasData(
                new Currency
                {
                    Id = 1,
                    Name = "Dolar",
                    Symbol = "USD",
                    Description = "Waluta w USA"
                },
                new Currency
                {
                    Id = 2,
                    Name = "Euro",
                    Symbol = "EUR",
                    Description = "Waluta w niektórych krajach UE"
                },
                new Currency
                {
                    Id = 3,
                    Name = "Funt",
                    Symbol = "GBP",
                    Description = "Waluta w UK"
                },
                new Currency
                {
                    Id = 4,
                    Name = "Polska złotówka",
                    Symbol = "PLN",
                    Description = "Waluta w Polsce"
                },
                new Currency
                {
                    Id = 5,
                    Name = "Dolar australijski",
                    Symbol = "AUD",
                    Description = "Waluta w Australii"
                },
                new Currency
                {
                    Id = 6,
                    Name = "Forint",
                    Symbol = "HUF",
                    Description = "Waluta na Węgrzech"
                },
                new Currency
                {
                    Id = 7,
                    Name = "Frank szwajcarski",
                    Symbol = "CHF",
                    Description = "Waluta w Szwajcarii"
                },
                new Currency
                {
                    Id = 8,
                    Name = "Jen",
                    Symbol = "JPY",
                    Description = "Waluta w Japonii"
                },
                new Currency
                {
                    Id = 9,
                    Name = "Korona czeska",
                    Symbol = "CZK",
                    Description = "Waluta w Czechach"
                },
                new Currency
                {
                    Id = 10,
                    Name = "Korona duńska",
                    Symbol = "DKK",
                    Description = "Waluta w Danii"
                },
                new Currency
                {
                    Id = 11,
                    Name = "Korona norweska",
                    Symbol = "NOK",
                    Description = "Waluta w Norwegii"
                },
                new Currency
                {
                    Id = 12,
                    Name = "Korona szwedzka",
                    Symbol = "SEK",
                    Description = "Waluta w Szwecji"
                },
                new Currency
                {
                    Id = 13,
                    Name = "Dolar kanadyjski",
                    Symbol = "CAD",
                    Description = "Waluta w Kanadzie"
                });

        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "679381f2-06a1-4e22-beda-179e8e9e3236",
                RoleId = 1,
                UserName = "TestUser1",
                NormalizedUserName = "TESTUSER1",
                Email = "test1@mail.com",
                NormalizedEmail = "TEST1@MAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEIR44hzbnj/pCIqsHG4vIPm/ARO5F+qPlxQp9Wjhn+EBi/q73B+RlmXZNV+yUOvgPQ==",
                Address = "Warszawa,",
                IdentityNumber = 997,
                IdNumber = "EZ12345",
                IdExpiryDate = new DateTime(2030, 01, 01),
                IdIssueDate = new DateTime(2020, 01, 01)
            });

        modelBuilder.Entity<ExchangeRate>().HasData(
            new ExchangeRate
            {
                Id = 1,
                FromCurrencyId = 4,
                ToCurrencyId = 1,
                Rate = 4,
                Direction = Direction.Buy
}           );

        modelBuilder.Entity<UserCurrencyAmount>().HasData(
            new UserCurrencyAmount
            {
                Id = 1,
                UserId = "679381f2-06a1-4e22-beda-179e8e9e3236",
                CurrencyId = 4,
                Amount = 100
            });

        modelBuilder.Entity<UserExchangeHistory>().HasData(
            new UserExchangeHistory
            {
                Id = 1,
                UserId = "679381f2-06a1-4e22-beda-179e8e9e3236",
                RateId = 1,
                Amount = 20,
                AccountId = 1,
                PaymentStatus = PaymentStatus.Completed,
                PaymentType = PaymentType.Blik
            });
    }
}