using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.SignOut;

public record SignOutCommand(string accessToken) : IRequest;