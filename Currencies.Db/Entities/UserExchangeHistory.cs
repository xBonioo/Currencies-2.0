using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Infrastructure;

namespace Currencies.Db.Entities;

public sealed class UserExchangeHistory : ICreatable
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public int? RateId { get; set; }
    public decimal Amount { get; set; }
    public int AccountId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentType? PaymentType { get; set; }
    public DateTime CreatedOn { get; set; }

    public ApplicationUser User { get; set; }
    public ExchangeRate Rate { get; set; }
    public UserCurrencyAmount Account { get; set; }
}
