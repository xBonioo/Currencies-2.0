using Currencies.Abstractions.Contracts.Enum;

namespace Currencies.Application.ModelDtos.User.ExchangeHistory;

public class BaseUserExchangeHistoryDto
{
    public string UserId { get; set; } = null!;
    public int? RateId { get; set; }
    public decimal Amount { get; set; }
    public int AccountId { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentType? PaymentType { get; set; }
}
