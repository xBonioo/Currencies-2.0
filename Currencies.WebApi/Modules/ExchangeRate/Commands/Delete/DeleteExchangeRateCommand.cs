using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Delete;

public class DeleteExchangeRateCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}