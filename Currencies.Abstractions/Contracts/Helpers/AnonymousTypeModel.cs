namespace Currencies.Abstractions.Contracts.Helpers;

public class AnonymousTypeModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Symbol { get; set; }
    public string? Description { get; set; }
    public decimal? RateDirection0 { get; set; }
    public decimal? RateDirection1 { get; set; }
    public bool IsActive { get; set; }
}
