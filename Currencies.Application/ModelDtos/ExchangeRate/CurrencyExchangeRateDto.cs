namespace Currencies.Application.ModelDtos.ExchangeRate;

public class CurrencyExchangeRateDto
{
    public string Code { get; set; } = null!;
    public decimal Ask { get; set; }
    public decimal Bid { get; set; }
}