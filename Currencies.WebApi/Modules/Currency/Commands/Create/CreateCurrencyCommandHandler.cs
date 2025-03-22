using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Create;

public class CreateCurrencyCommandHandler(ICurrencyService currencyService)
    : IRequestHandler<CreateCurrencyCommand, CurrencyDto?>
{
    public async Task<CurrencyDto?> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return await currencyService.CreateAsync(request.Data, cancellationToken);
    }
}