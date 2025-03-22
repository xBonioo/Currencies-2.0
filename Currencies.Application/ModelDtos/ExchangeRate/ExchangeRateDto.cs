namespace Currencies.Application.ModelDtos.ExchangeRate;

public class ExchangeRateDto : BaseExchangeRateDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
