using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetSingle;

public class GetSingleUserExchangeHistoryQueryHandler : IRequestHandler<GetSingleUserExchangeHistoryQuery, UserExchangeHistoryDto?>
{
    private readonly IUserExchangeHistoryService _userExchangeHistoryService;
    private readonly IMapper _mapper;

    public GetSingleUserExchangeHistoryQueryHandler(IUserExchangeHistoryService userExchangeHistoryService, IMapper mapper)
    {
        _userExchangeHistoryService = userExchangeHistoryService;
        _mapper = mapper;
    }

    public async Task<UserExchangeHistoryDto?> Handle(GetSingleUserExchangeHistoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _userExchangeHistoryService.GetByIdAsync(request.Id, cancellationToken);
        if (result == null)
        {
            return null;
        }

        return _mapper.Map<UserExchangeHistoryDto>(result);
    }
}