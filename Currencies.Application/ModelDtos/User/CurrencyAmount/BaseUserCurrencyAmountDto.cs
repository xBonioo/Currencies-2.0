namespace Currencies.Application.ModelDtos.User.CurrencyAmount;

public class BaseUserCurrencyAmountDto
{
    public string UserId { get; set; } = null!;
    public int CurrencyId { get; set; }
    public decimal Amount { get; set; }
    public bool IsActive { get; set; }
}
