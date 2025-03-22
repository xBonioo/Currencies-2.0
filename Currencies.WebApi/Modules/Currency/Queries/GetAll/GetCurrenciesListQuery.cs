using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetAll;

public class GetCurrenciesListQuery(FilterCurrencyDto filter) : IRequest<PageResult<CurrencyDto>>
{
    public FilterCurrencyDto Filter = filter;
}