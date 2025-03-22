using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Create;

public class CreateExchangeRateCommand : IRequest<List<ExchangeRateDto>>
{
    public DateTime Date { get; set; }

    public CreateExchangeRateCommand(DateTime date)
    {
        Date = date;
    }
}