using AutoMapper;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingle;

public class GetSinglExchangeRateQueryHandler : IRequestHandler<GetSingleExchangeRateQuery, List<ExchangeRateDto>>
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IMapper _mapper;

    public GetSinglExchangeRateQueryHandler(IExchangeRateService exchangeRateService, IMapper mapper)
    {
        _exchangeRateService = exchangeRateService;
        _mapper = mapper;
    }

    public async Task<List<ExchangeRateDto>> Handle(GetSingleExchangeRateQuery request, CancellationToken cancellationToken)
    {
        var result = await _exchangeRateService.GetByCurrencyIdAsync(request.id, request.direction, cancellationToken);
        if (result == null)
        {
            return null;
        }

        return _mapper.Map<List<ExchangeRateDto>>(result);
    }
}