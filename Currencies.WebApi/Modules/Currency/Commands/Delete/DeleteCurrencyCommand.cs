using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Delete;

public class DeleteCurrencyCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}