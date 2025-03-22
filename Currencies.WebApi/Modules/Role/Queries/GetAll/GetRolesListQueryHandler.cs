using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetAll;

public class GetRolesListQueryHandler : IRequestHandler<GetRolesListQuery, PageResult<RoleDto>?>
{
    private readonly IRoleService _roleService;

    public GetRolesListQueryHandler(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<PageResult<RoleDto>?> Handle(GetRolesListQuery request, CancellationToken cancellationToken)
    {
        return await _roleService.GetAllRolesAsync(request.Filter, cancellationToken);
    }
}