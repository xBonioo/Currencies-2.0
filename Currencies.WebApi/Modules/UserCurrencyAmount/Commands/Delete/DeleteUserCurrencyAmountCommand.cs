using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Delete;

public class DeleteUserCurrencyAmountCommand : IRequest<bool>
{
    public int Id { get; set; }

    public DeleteUserCurrencyAmountCommand(int id)
    {
        Id = id;
    }
}
