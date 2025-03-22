using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingleFromCurrency;

public class GetSingleExchangeRateFromCurrencyQueryHandler(IExchangeRateService exchangeRateService, IMapper mapper)
    : IRequestHandler<GetSingleExchangeRateFromCurrencyQuery, (ExchangeRateDto?, ExchangeRateDto?)>
{
    public async Task<(ExchangeRateDto?, ExchangeRateDto?)> Handle(GetSingleExchangeRateFromCurrencyQuery request, CancellationToken cancellationToken)
    {
        var result = await exchangeRateService.GetByIdFromCurrencyAsync(request.fromId, request.toId, cancellationToken);
        if (result == (null, null))
        {
            return (null, null);
        }

        var direction0Dto = result.Item1 != null ? mapper.Map<ExchangeRateDto>(result.Item1) : null;
        var direction1Dto = result.Item2 != null ? mapper.Map<ExchangeRateDto>(result.Item2) : null;

        return (direction0Dto, direction1Dto);
    }
}