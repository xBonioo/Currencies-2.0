using Currencies.Abstractions.Response;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.RefreshToken;

public record RefreshTokenCommand(string refreshToken, string accessToken) : IRequest<RefreshTokenResponse?>;