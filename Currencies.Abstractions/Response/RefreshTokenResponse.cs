using Currencies.Abstractions.Contracts.Helpers;

namespace Currencies.Abstractions.Response;

public class RefreshTokenResponse
{
    public RefreshToken? RefreshToken { get; set; }
    public string? AccessToken { get; set; }
    public string? UserId { get; set; }
}