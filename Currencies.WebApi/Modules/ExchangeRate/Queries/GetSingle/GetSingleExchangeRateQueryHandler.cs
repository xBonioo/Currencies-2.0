using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingle;

public class GetSinglExchangeRateQueryHandler(IExchangeRateService exchangeRateService, IMapper mapper)
    : IRequestHandler<GetSingleExchangeRateQuery, List<ExchangeRateDto>>
{
    public async Task<List<ExchangeRateDto>?> Handle(GetSingleExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var result = await exchangeRateService.GetByCurrencyIdAsync(request.id, request.direction, cancellationToken);
        if (result == null)
        {
            return null;
        }

        return mapper.Map<List<ExchangeRateDto>>(result);
    }
}