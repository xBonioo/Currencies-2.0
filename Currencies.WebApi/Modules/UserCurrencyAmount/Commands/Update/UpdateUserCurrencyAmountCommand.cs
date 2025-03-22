using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;

public class UpdateUserCurrencyAmountCommand : IRequest<UserCurrencyAmountDto>
{
    public int Id { get; set; }
    public BaseUserCurrencyAmountDto Dto { get; set; }

    public UpdateUserCurrencyAmountCommand(int id, BaseUserCurrencyAmountDto dto)
    {
        Id = id;
        Dto = dto;
    }
}
