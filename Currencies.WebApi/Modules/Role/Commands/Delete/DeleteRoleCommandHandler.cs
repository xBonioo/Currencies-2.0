using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Commands.Delete;

public class DeleteRoleCommandHandler(IRoleService roleService) : IRequestHandler<DeleteRoleCommand, bool>
{
    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        return await roleService.DeleteAsync(request.Id, cancellationToken);
    }
}