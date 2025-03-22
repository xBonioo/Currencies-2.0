namespace Currencies.Abstractions.Contracts.Helpers;

public class RefreshToken
{
    public string? Token { get; set; }
    public DateTime ValidUntil { get; set; }
}
