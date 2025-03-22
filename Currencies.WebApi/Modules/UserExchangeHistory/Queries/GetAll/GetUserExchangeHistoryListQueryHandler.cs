using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetAll;

public class GetUserExchangeHistoryListQueryHandler : IRequestHandler<GetUserExchangeHistoryListQuery, PageResult<UserExchangeHistoryDto>?>
{
    private readonly IUserExchangeHistoryService _userExchangeHistoryService;

    public GetUserExchangeHistoryListQueryHandler(IUserExchangeHistoryService userExchangeHistoryService)
    {
        _userExchangeHistoryService = userExchangeHistoryService;
    }

    public async Task<PageResult<UserExchangeHistoryDto>?> Handle(GetUserExchangeHistoryListQuery request, CancellationToken cancellationToken)
    {
        return await _userExchangeHistoryService.GetAllUserExchangeHistoryServiceiesAsync(request.Filter, cancellationToken);
    }
}