using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Convert;

public class ConvertUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    : IRequestHandler<ConvertUserCurrencyAmountCommand, UserCurrencyAmountDto>
{
    public async Task<UserCurrencyAmountDto?> Handle(ConvertUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await userCurrencyAmountService.ConvertAsync(request.Data, cancellationToken);
    }
}
