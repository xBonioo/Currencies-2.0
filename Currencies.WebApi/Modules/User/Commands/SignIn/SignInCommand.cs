using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.SignIn;

public record SignInCommand(SignInDto dto) : IRequest<RefreshTokenResponse?>;