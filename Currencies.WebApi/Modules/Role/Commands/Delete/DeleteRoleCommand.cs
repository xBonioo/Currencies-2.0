using MediatR;

namespace Currencies.WebApi.Modules.Role.Commands.Delete;

public class DeleteRoleCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteRoleCommand(int id)
    {
        Id = id;
    }
}