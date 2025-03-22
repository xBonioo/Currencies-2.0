using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetAll;

public class GetCurrenciesListQueryHandler(ICurrencyService currencyService)
    : IRequestHandler<GetCurrenciesListQuery, PageResult<CurrencyDto>?>
{
    public async Task<PageResult<CurrencyDto>?> Handle(GetCurrenciesListQuery request, CancellationToken cancellationToken)
    {
        return await currencyService.GetAllCurrenciesAsync(request.Filter, cancellationToken);
    }
}