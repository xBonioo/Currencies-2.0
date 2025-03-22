using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.SignOut;

public class SignOutHandler(IUserService userService) : IRequestHandler<SignOutCommand>
{
    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await userService.SignOutUserAsync(request.accessToken, cancellationToken);
    }
}