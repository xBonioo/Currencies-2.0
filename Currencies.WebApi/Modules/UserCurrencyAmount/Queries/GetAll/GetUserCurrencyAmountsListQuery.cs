using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetAll;

public class GetUserCurrencyAmountsListQuery : IRequest<PageResult<UserCurrencyAmountDto>>
{
    public FilterUserCurrencyAmountDto Filter;

    public GetUserCurrencyAmountsListQuery(FilterUserCurrencyAmountDto filter)
    {
        Filter = filter;
    }
}
