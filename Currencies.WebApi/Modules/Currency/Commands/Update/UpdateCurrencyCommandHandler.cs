using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Commands.Update;

public class UpdateCurrencyCommandHandler(ICurrencyService currencyService)
    : IRequestHandler<UpdateCurrencyCommand, CurrencyDto>
{
    public async Task<CurrencyDto?> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        return await currencyService.UpdateAsync(request.Id, request.Dto, cancellationToken);
    }
}