using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.SignIn;

public class SignInHandler : IRequestHandler<SignInCommand, RefreshTokenResponse?>
{
    private readonly IUserService _userService;

    public SignInHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<RefreshTokenResponse?> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        return await _userService.SignInUserAsync(request.dto, cancellationToken);
    }
}