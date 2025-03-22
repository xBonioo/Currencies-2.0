namespace Currencies.Application.ModelDtos.User.ExchangeHistory;

public sealed class FilterUserExchangeHistoryDto : FilterDto
{
    public string UserId { get; set; } = null!;
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}
