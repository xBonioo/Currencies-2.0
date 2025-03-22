namespace Currencies.Application.ModelDtos.Currency;

public sealed class CurrencyDto : BaseCurrencyDto
{
    public decimal RateDirection0 { get; set; }
    public decimal RateDirection1 { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}