using Currencies.Abstractions.Requests;
using Currencies.Application.ModelDtos.User;
using MediatR;

namespace Currencies.WebApi.Modules.User.Queries.GetUser;

public record GetUserInformationQuery(GetUserRequest request) : IRequest<UserDto>;