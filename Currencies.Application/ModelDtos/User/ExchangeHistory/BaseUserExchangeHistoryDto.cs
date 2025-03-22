using Currencies.Abstractions.Contracts.Enum;

namespace Currencies.Application.ModelDtos.User.ExchangeHistory;

public class BaseUserExchangeHistoryDto
{
    public string UserID { get; set; } = null!;
    public int? RateID { get; set; }
    public decimal Amount { get; set; }
    public int AccountID { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public PaymentType? PaymentType { get; set; }
}
