using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetSingle;

public record GetSingleRoleQuery(int id) : IRequest<RoleDto>;