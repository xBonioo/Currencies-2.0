using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Update;

public class UpdateExchangeRateCommandHandler : IRequestHandler<UpdateExchangeRateCommand, ExchangeRateDto>
{
    private readonly IExchangeRateService _exchangeRateService;

    public UpdateExchangeRateCommandHandler(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<ExchangeRateDto?> Handle(UpdateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        return await _exchangeRateService.UpdateAsync(request.Id, request.Dto, cancellationToken);
    }
}