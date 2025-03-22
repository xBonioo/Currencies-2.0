using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Convert;

public class ConvertUserCurrencyAmountCommandHandler : IRequestHandler<ConvertUserCurrencyAmountCommand, UserCurrencyAmountDto>
{
    private readonly IUserCurrencyAmountService _userCurrencyAmountService;

    public ConvertUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    {
        _userCurrencyAmountService = userCurrencyAmountService;
    }

    public async Task<UserCurrencyAmountDto?> Handle(ConvertUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await _userCurrencyAmountService.ConvertAsync(request.Data, cancellationToken);
    }
}
