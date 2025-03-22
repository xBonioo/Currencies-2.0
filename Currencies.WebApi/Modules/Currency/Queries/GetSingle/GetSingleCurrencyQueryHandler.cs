using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetSingle;

public class GetSingleCurrencyQueryHandler(ICurrencyService currencyService, IMapper mapper)
    : IRequestHandler<GetSingleCurrencyQuery, CurrencyDto?>
{
    public async Task<CurrencyDto?> Handle(GetSingleCurrencyQuery request, CancellationToken cancellationToken)
    {
        var result = await currencyService.GetByIdAsync(request.id, cancellationToken);
        if (result is not { IsActive: true })
        {
            return null;
        }

        return mapper.Map<CurrencyDto>(result);
    }
}