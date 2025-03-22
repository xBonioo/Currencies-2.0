using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Create;

public class CreateUserCurrencyAmountCommand : IRequest<UserCurrencyAmountDto>
{
    public BaseUserCurrencyAmountDto? Data { get; set; } = null!;

    public CreateUserCurrencyAmountCommand(BaseUserCurrencyAmountDto? dto)
    {
        Data = dto;
    }
}
