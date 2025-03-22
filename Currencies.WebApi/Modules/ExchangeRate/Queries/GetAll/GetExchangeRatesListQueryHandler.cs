using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetAll;

public class GetExchangeRatesListQueryHandler(IExchangeRateService exchangeRateService)
    : IRequestHandler<GetExchangeRatesListQuery, PageResult<ExchangeRateDto>?>
{
    public async Task<PageResult<ExchangeRateDto>?> Handle(GetExchangeRatesListQuery request, CancellationToken cancellationToken)
    {
        return await exchangeRateService.GetAllExchangeRateAsync(request.Filter, cancellationToken);
    }
}