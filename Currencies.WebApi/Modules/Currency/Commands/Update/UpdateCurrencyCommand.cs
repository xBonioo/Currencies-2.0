using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Update;

public class UpdateCurrencyCommand(int id, BaseCurrencyDto dto) : IRequest<CurrencyDto>
{
    public int Id { get; set; } = id;
    public BaseCurrencyDto Dto { get; set; } = dto;
}