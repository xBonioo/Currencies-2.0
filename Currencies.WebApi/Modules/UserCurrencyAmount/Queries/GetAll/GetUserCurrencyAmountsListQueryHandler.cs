using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetAll;

public class GetUserCurrencyAmountsListQueryHandler : IRequestHandler<GetUserCurrencyAmountsListQuery, PageResult<UserCurrencyAmountDto>?>
{
    private readonly IUserCurrencyAmountService _userCurrencyAmountService;

    public GetUserCurrencyAmountsListQueryHandler(IUserCurrencyAmountService userCurrencyAmountService)
    {
        _userCurrencyAmountService = userCurrencyAmountService;
    }

    public async Task<PageResult<UserCurrencyAmountDto>?> Handle(GetUserCurrencyAmountsListQuery request, CancellationToken cancellationToken)
    {
        return await _userCurrencyAmountService.GetAllUserCurrencyAmountsAsync(request.Filter, cancellationToken);
    }
}
