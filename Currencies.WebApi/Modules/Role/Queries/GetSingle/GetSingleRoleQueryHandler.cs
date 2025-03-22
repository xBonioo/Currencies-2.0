using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetSingle;

public class GetSinglRoleQueryHandler : IRequestHandler<GetSingleRoleQuery, RoleDto?>
{
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;

    public GetSinglRoleQueryHandler(IRoleService roleService, IMapper mapper)
    {
        _roleService = roleService;
        _mapper = mapper;
    }

    public async Task<RoleDto?> Handle(GetSingleRoleQuery request, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetByIdAsync(request.id, cancellationToken);
        if (result == null || !result.IsActive)
        {
            return null;
        }

        return _mapper.Map<RoleDto>(result);
    }
}