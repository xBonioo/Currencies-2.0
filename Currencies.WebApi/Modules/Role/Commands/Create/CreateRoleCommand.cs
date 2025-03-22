using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Commands.Create;

public class CreateRoleCommand : IRequest<RoleDto>
{
    public BaseRoleDto Data { get; set; } = null!;

    public CreateRoleCommand(BaseRoleDto dto)
    {
        Data = dto;
    }
}