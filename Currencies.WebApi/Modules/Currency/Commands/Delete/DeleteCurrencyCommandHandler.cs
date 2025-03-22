using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Delete;

public class DeleteCurrencyCommandHandler(ICurrencyService currencyService)
    : IRequestHandler<DeleteCurrencyCommand, bool>
{
    public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        return await currencyService.DeleteAsync(request.Id, cancellationToken);
    }
}