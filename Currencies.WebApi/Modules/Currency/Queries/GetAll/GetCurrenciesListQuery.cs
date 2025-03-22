using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetAll;

public class GetCurrenciesListQuery : IRequest<PageResult<CurrencyDto>>
{
    public FilterCurrencyDto Filter;

    public GetCurrenciesListQuery(FilterCurrencyDto filter)
    {
        Filter = filter;
    }
}