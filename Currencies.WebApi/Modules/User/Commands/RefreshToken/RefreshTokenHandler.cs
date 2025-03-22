using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse?>
{
    private readonly IUserService _userService;

    public RefreshTokenHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<RefreshTokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await _userService.RefreshTokenAsync(request.refreshToken, request.accessToken, cancellationToken);
    }
}