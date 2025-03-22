using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetAll;

public class GetUserExchangeHistoryListQuery : IRequest<PageResult<UserExchangeHistoryDto>>
{
    public FilterUserExchangeHistoryDto Filter;

    public GetUserExchangeHistoryListQuery(FilterUserExchangeHistoryDto filter)
    {
        Filter = filter;
    }
}
