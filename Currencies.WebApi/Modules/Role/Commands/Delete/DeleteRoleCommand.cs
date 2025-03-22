using MediatR;

namespace Currencies.WebApi.Modules.Role.Commands.Delete;

public class DeleteRoleCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}