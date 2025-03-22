using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Commands.Add;

public class AddUserExchangeHistoryCommand : IRequest<bool>
{
    public UserExchangeHistoryDto? Data { get; set; } = null!;

    public AddUserExchangeHistoryCommand(UserExchangeHistoryDto? dto)
    {
        Data = dto;
    }
}
