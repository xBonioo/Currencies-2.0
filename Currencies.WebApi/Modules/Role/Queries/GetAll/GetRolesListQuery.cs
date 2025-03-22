using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetAll;

public class GetRolesListQuery : IRequest<PageResult<RoleDto>>
{
    public FilterRoleDto Filter;

    public GetRolesListQuery(FilterRoleDto filter)
    {
        Filter = filter;
    }
}