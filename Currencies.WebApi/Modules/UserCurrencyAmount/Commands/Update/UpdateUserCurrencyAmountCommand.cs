using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;

public class UpdateUserCurrencyAmountCommand(int id, BaseUserCurrencyAmountDto dto) : IRequest<UserCurrencyAmountDto>
{
    public int Id { get; set; } = id;
    public BaseUserCurrencyAmountDto Dto { get; set; } = dto;
}
