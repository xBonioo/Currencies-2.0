using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetAll;

public class GetUserExchangeHistoryListQuery(FilterUserExchangeHistoryDto filter)
    : IRequest<PageResult<UserExchangeHistoryDto>>
{
    public FilterUserExchangeHistoryDto Filter = filter;
}
