namespace Currencies.Application.ModelDtos.User.CurrencyAmount;

public sealed class ConvertUserCurrencyAmountDto
{
    public string UserId { get; set; } = null!;
    public int FromCurrencyId { get; set; }
    public int ToCurrencyId { get; set; }
    public decimal Amount { get; set; }
}
