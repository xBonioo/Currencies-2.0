namespace Currencies.Application.ModelDtos.User.ExchangeHistory;

public sealed class UserExchangeHistoryDto : BaseUserExchangeHistoryDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
}
