namespace Currencies.Application.ModelDtos.Currency;

public class BaseCurrencyDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Symbol { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}