using Currencies.Abstractions.Forms;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetEditForm;

public record GetRoleEditFormQuery(int id) : IRequest<RoleEditForm>;