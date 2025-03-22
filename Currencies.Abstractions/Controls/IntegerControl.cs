namespace Currencies.Abstractions.Controls;

public class IntegerControl
{
    public bool IsRequired { get; set; }
    public int Value { get; set; }
    public int? MinValue { get; set; }
    public int? MaxValue { get; set; }
}
