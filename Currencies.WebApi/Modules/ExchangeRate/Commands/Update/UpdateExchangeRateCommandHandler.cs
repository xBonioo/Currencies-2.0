using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Update;

public class UpdateExchangeRateCommandHandler(IExchangeRateService exchangeRateService)
    : IRequestHandler<UpdateExchangeRateCommand, ExchangeRateDto>
{
    public async Task<ExchangeRateDto?> Handle(UpdateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        return await exchangeRateService.UpdateAsync(request.Id, request.Dto, cancellationToken);
    }
}