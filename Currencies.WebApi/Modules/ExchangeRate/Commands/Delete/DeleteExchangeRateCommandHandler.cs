using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Commands.Delete;

public class DeleteExchangeRateCommandHandler(IExchangeRateService exchangeRateService)
    : IRequestHandler<DeleteExchangeRateCommand, bool>
{
    public async Task<bool> Handle(DeleteExchangeRateCommand request, CancellationToken cancellationToken)
    {
        return await exchangeRateService.DeleteAsync(request.Id, cancellationToken);
    }
}