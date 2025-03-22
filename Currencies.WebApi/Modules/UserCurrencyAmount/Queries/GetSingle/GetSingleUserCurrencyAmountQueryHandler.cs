using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetSingle;

public class GetSinglUserCurrencyAmountQueryHandler(
    IUserCurrencyAmountService userCurrencyAmountService,
    IMapper mapper)
    : IRequestHandler<GetSingleUserCurrencyAmountQuery, List<UserCurrencyAmountDto>>
{
    public async Task<List<UserCurrencyAmountDto>?> Handle(GetSingleUserCurrencyAmountQuery request, CancellationToken cancellationToken)
    {
        var result = await userCurrencyAmountService.GetByUserIdAsync(request.Id, cancellationToken);
        if (result == null)
        {
            return null;
        }

        return mapper.Map<List<UserCurrencyAmountDto>>(result);
    }
}
