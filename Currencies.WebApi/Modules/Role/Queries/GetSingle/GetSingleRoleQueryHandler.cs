using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetSingle;

public class GetSinglRoleQueryHandler(IRoleService roleService, IMapper mapper)
    : IRequestHandler<GetSingleRoleQuery, RoleDto?>
{
    public async Task<RoleDto?> Handle(GetSingleRoleQuery request, CancellationToken cancellationToken)
    {
        var result = await roleService.GetByIdAsync(request.id, cancellationToken);
        if (result is not { IsActive: true })
        {
            return null;
        }

        return mapper.Map<RoleDto>(result);
    }
}