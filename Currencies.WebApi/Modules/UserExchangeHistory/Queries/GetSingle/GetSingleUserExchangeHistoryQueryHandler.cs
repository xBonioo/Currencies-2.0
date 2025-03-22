using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetSingle;

public class GetSingleUserExchangeHistoryQueryHandler(
    IUserExchangeHistoryService userExchangeHistoryService,
    IMapper mapper)
    : IRequestHandler<GetSingleUserExchangeHistoryQuery, UserExchangeHistoryDto?>
{
    public async Task<UserExchangeHistoryDto?> Handle(GetSingleUserExchangeHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = await userExchangeHistoryService.GetByIdAsync(request.Id, cancellationToken);
        if (result == null)
        {
            return null;
        }

        return mapper.Map<UserExchangeHistoryDto>(result);
    }
}