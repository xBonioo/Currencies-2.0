using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetAll;

public class GetExchangeRatesListQuery(FilterExchangeRateDto filter) : IRequest<PageResult<ExchangeRateDto>>
{
    public FilterExchangeRateDto Filter = filter;
}