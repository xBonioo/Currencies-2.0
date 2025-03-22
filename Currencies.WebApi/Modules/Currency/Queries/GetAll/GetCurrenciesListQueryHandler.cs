using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetAll;

public class GetCurrenciesListQueryHandler : IRequestHandler<GetCurrenciesListQuery, PageResult<CurrencyDto>?>
{
    private readonly ICurrencyService _currencyService;

    public GetCurrenciesListQueryHandler(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task<PageResult<CurrencyDto>?> Handle(GetCurrenciesListQuery request, CancellationToken cancellationToken)
    {
        return await _currencyService.GetAllCurrenciesAsync(request.Filter, cancellationToken);
    }
}