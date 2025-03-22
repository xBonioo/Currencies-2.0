using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Update;

public class UpdateExchangeRateCommand(int id, BaseExchangeRateDto dto) : IRequest<ExchangeRateDto>
{
    public int Id { get; set; } = id;
    public BaseExchangeRateDto Dto { get; set; } = dto;
}