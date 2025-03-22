using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User;
using Currencies.Db;
using Currencies.Db.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly TableContext _dbContext;
    private readonly ITokenService _tokenService;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        TableContext dbContext, ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
        _tokenService = tokenService;
    }

    public async Task<UserDto> RegisterUserAsync(RegisterUserDto registerUserDto, CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = registerUserDto.UserName,
            Email = registerUserDto.Email,
            EmailConfirmed = true,
            RoleId = 2,
            IsActive = true,
            Adres = registerUserDto.Adres,
            IdentityNumber = registerUserDto.IdentityNumber,
            IDNumber = registerUserDto.IDNumber,
            IDExpiryDate = registerUserDto.IDExpiryDate,
            IDIssueDate = registerUserDto.IDIssueDate
        };

        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.Count() > 1)
                {
                    throw new AggregateException("Multiple errors occured while creating user.",
                        result.Errors.Select(x => new Exception(x.Description)).ToList());
                }
                throw new Exception("Error occured while creating user: " + result.Errors.First().Description);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                IsActive = user.IsActive
            };
        }
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync(string refreshToken, string accessToken, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        var userRefreshTokenRecord = _dbContext.UserTokens.FirstOrDefault(u => u.Value == refreshToken);

        if (userRefreshTokenRecord == null)
        {
            return null;
        }

        var user = _dbContext.Users.FirstOrDefault(u => u.Id == userRefreshTokenRecord.UserId);
        if (user == null)
        {
            return null;
        }

        if (!userRefreshTokenRecord.IsActive)
        {
            return null;
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        userRefreshTokenRecord.Value = newRefreshToken.Token;
        userRefreshTokenRecord.ValidUntil = newRefreshToken.ValidUntil;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new RefreshTokenResponse
        {
            RefreshToken = newRefreshToken,
            AccessToken = newAccessToken
        };

        return response;
    }

    public async Task<RefreshTokenResponse?> SignInUserAsync(SignInDto signInDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
                        .Include(u => u.Role)
                        .FirstOrDefaultAsync(u => u.UserName == signInDto.Username);

        if (user is null)
        {
            return null;
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, signInDto.Password, false, false);

        if (!signInResult.Succeeded)
        {
            return null;
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var userRefreshTokenRecord = _dbContext.UserTokens.FirstOrDefault(u => u.UserId == user.Id);
        if (userRefreshTokenRecord is null)
        {
            _dbContext.UserTokens.Add(new TokenUser
            {
                UserId = user.Id,
                LoginProvider = "Own",
                Name = "RefreshToken",
                Value = newRefreshToken.Token,
                ValidUntil = newRefreshToken.ValidUntil,
            });
        }
        else
        {
            userRefreshTokenRecord.Value = newRefreshToken.Token;
            userRefreshTokenRecord.ValidUntil = newRefreshToken.ValidUntil;
        }
        await _dbContext.SaveChangesAsync(cancellationToken);
        var response = new RefreshTokenResponse
        {
            RefreshToken = newRefreshToken,
            AccessToken = accessToken,
            UserId = user.Id
        };

        return response;
    }

    public async Task<bool> SignOutUserAsync(string accessToken, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);

        var userId = principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

        var userRefreshTokenRecord = _dbContext.UserTokens.Single(u => u.UserId == userId);

        userRefreshTokenRecord.Value = null;
        userRefreshTokenRecord.ValidUntil = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
