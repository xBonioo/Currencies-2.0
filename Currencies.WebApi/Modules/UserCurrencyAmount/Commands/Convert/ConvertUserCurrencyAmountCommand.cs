using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Convert;

public class ConvertUserCurrencyAmountCommand : IRequest<UserCurrencyAmountDto>
{
    public ConvertUserCurrencyAmountDto Data { get; set; } = null!;

    public ConvertUserCurrencyAmountCommand(ConvertUserCurrencyAmountDto dto)
    {
        Data = dto;
    }
}
