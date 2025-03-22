using Currencies.Application.ModelDtos.Role;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Commands.Update;

public class UpdateRoleCommand(int id, BaseRoleDto dto) : IRequest<RoleDto>
{
    public int Id { get; set; } = id;
    public BaseRoleDto Dto { get; set; } = dto;
}