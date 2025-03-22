using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetAll;

public class GetRolesListQueryHandler(IRoleService roleService)
    : IRequestHandler<GetRolesListQuery, PageResult<RoleDto>?>
{
    public async Task<PageResult<RoleDto>?> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
    {
        return await roleService.GetAllRolesAsync(request.Filter, cancellationToken);
    }
}