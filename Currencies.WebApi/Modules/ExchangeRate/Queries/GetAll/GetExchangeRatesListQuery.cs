using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetAll;

public class GetExchangeRatesListQuery : IRequest<PageResult<ExchangeRateDto>>
{
    public FilterExchangeRateDto Filter;

    public GetExchangeRatesListQuery(FilterExchangeRateDto filter)
    {
        Filter = filter;
    }
}