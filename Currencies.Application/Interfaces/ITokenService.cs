using System.Security.Claims;
using Currencies.Abstractions.Contracts.Helpers;

namespace Currencies.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    RefreshToken GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
}
