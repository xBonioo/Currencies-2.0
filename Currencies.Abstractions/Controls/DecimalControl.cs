namespace Currencies.Abstractions.Controls;

public class DecimalControl
{
    public bool IsRequired { get; set; }
    public decimal Value { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
}
