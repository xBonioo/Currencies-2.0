using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Commands.Create;

public class CreateRoleCommandHandler(IRoleService roleService) : IRequestHandler<CreateRoleCommand, RoleDto?>
{
    public async Task<RoleDto?> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleService.CreateAsync(request.Data, cancellationToken);
    }
}