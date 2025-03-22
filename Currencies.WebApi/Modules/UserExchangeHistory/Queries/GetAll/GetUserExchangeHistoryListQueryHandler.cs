using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetAll;

public class GetUserExchangeHistoryListQueryHandler(IUserExchangeHistoryService userExchangeHistoryService)
    : IRequestHandler<GetUserExchangeHistoryListQuery, PageResult<UserExchangeHistoryDto>?>
{
    public async Task<PageResult<UserExchangeHistoryDto>?> Handle(GetUserExchangeHistoryListQuery request, CancellationToken cancellationToken)
    {
        return await userExchangeHistoryService.GetAllUserExchangeHistoryServiceiesAsync(request.Filter, cancellationToken);
    }
}