using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.RefreshToken;

public class RefreshTokenHandler(IUserService userService) : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse?>
{
    public async Task<RefreshTokenResponse?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        return await userService.RefreshTokenAsync(request.refreshToken, request.accessToken, cancellationToken);
    }
}