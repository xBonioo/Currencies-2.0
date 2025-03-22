using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Delete;

public class DeleteUserCurrencyAmountCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}
