namespace Currencies.Application.ModelDtos.User.CurrencyAmount;

public class UserCurrencyAmountDto : BaseUserCurrencyAmountDto
{
    public int Id { get; set; }
    public int? RateId { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
