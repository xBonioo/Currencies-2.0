using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Create;

public class CreateCurrencyCommand : IRequest<CurrencyDto>
{
    public BaseCurrencyDto Data { get; set; } = null!;

    public CreateCurrencyCommand(BaseCurrencyDto dto)
    {
        Data = dto;
    }
}