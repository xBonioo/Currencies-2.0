using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.SignIn;

public class SignInHandler(IUserService userService) : IRequestHandler<SignInCommand, RefreshTokenResponse?>
{
    public async Task<RefreshTokenResponse?> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await userService.SignInUserAsync(request.dto, cancellationToken);
    }
}