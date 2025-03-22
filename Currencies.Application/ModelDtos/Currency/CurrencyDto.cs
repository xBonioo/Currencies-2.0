namespace Currencies.Application.ModelDtos.Currency;

public class CurrencyDto : BaseCurrencyDto
{
    public decimal Rate_Direction_0 { get; set; }
    public decimal Rate_Direction_1 { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}