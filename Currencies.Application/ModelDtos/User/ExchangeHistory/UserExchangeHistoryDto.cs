namespace Currencies.Application.ModelDtos.User.ExchangeHistory;

public class UserExchangeHistoryDto : BaseUserExchangeHistoryDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
}
