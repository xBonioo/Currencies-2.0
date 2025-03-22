using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetAll;

public class GetUserCurrencyAmountsListQueryHandler(IUserCurrencyAmountService userCurrencyAmountService)
    : IRequestHandler<GetUserCurrencyAmountsListQuery, PageResult<UserCurrencyAmountDto>?>
{
    public async Task<PageResult<UserCurrencyAmountDto>?> Handle(GetUserCurrencyAmountsListQuery request, CancellationToken cancellationToken)
    {
        return await userCurrencyAmountService.GetAllUserCurrencyAmountsAsync(request.Filter, cancellationToken);
    }
}
