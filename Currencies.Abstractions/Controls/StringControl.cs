namespace Currencies.Abstractions.Controls;

public class StringControl
{
    public bool IsRequired { get; set; }
    public string? Value { get; set; }
    public int? MinLenght { get; set; }
    public int? MaxLenght { get; set; }
}
