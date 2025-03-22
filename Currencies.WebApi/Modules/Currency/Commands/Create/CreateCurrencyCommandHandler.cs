using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Create;

public class CreateCurrencyCommandHandler : IRequestHandler<CreateCurrencyCommand, CurrencyDto?>
{
    private readonly ICurrencyService _currencyService;

    public CreateCurrencyCommandHandler(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task<CurrencyDto?> Handle(CreateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return await _currencyService.CreateAsync(request.Data, cancellationToken);
    }
}