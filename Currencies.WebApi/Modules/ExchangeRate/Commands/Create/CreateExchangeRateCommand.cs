using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Create;

public class CreateExchangeRateCommand(DateTime date) : IRequest<List<ExchangeRateDto>>
{
    public DateTime Date { get; set; } = date;
}