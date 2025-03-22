using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingleFromCurrency;

public class GetSingleExchangeRateFromCurrencyQueryHandler : IRequestHandler<GetSingleExchangeRateFromCurrencyQuery, (ExchangeRateDto?, ExchangeRateDto?)>
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IMapper _mapper;

    public GetSingleExchangeRateFromCurrencyQueryHandler(IExchangeRateService exchangeRateService, IMapper mapper)
    {
        _exchangeRateService = exchangeRateService;
        _mapper = mapper;
    }

    public async Task<(ExchangeRateDto?, ExchangeRateDto?)> Handle(GetSingleExchangeRateFromCurrencyQuery request, CancellationToken cancellationToken)
    {
        var result = await _exchangeRateService.GetByIdFromCurrencyAsync(request.fromId, request.toId, cancellationToken);
        if (result == (null, null))
        {
            return (null, null);
        }

        var direction0Dto = result.Item1 != null ? _mapper.Map<ExchangeRateDto>(result.Item1) : null;
        var direction1Dto = result.Item2 != null ? _mapper.Map<ExchangeRateDto>(result.Item2) : null;

        return (direction0Dto, direction1Dto);
    }
}