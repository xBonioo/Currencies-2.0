using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Currencies.Abstractions.Contracts.Helpers;
using Currencies.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Currencies.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwt = new JwtSecurityToken(
            audience: Environment.GetEnvironmentVariable("Authentication:Audience") ?? _configuration["Authentication:Audience"],
            issuer: _configuration["Authentication:Issuer"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(5),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ValidUntil = DateTime.UtcNow.AddDays(2)
        };
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = _configuration["Authentication:Issuer"],
            ValidAudience = _configuration["Authentication:Audience"],
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
