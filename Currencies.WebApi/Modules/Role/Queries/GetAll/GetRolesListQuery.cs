using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetAll;

public class GetRolesListQuery(FilterRoleDto filter) : IRequest<PageResult<RoleDto>>
{
    public FilterRoleDto Filter = filter;
}