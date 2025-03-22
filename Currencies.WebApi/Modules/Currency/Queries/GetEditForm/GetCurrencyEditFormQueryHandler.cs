using Currencies.Abstractions.Controls;
using Currencies.Abstractions.Forms;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetEditForm;

public class GetCurrencyEditFormQueryHandler : IRequestHandler<GetCurrencyEditFormQuery, CurrencyEditForm?>
{
    private readonly ICurrencyService _currencyService;

    public GetCurrencyEditFormQueryHandler(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    public async Task<CurrencyEditForm?> Handle(GetCurrencyEditFormQuery request, CancellationToken cancellationToken)
    {
        var currency = await _currencyService.GetByIdAsync(request.id, cancellationToken);
        if (currency == null || !currency.IsActive)
        {
            var createForm = new CurrencyEditForm()
            {
                Name = new StringControl()
                {
                    IsRequired = true,
                    Value = string.Empty,
                    MinLenght = 1,
                    MaxLenght = 64
                },
                Symbol = new StringControl()
                {
                    IsRequired = true,
                    Value = string.Empty,
                    MinLenght = 1,
                    MaxLenght = 3
                },
                Description = new StringControl()
                {
                    IsRequired = false,
                    Value = null,
                    MinLenght = 1,
                    MaxLenght = 256
                },
                IsActive = new BoolControl()
                {
                    IsRequired = true,
                    Value = true
                }
            };

            return createForm;
        }

        var editForm = new CurrencyEditForm()
        {
            Name = new StringControl()
            {
                IsRequired = true,
                Value = currency.Name,
                MinLenght = 1,
                MaxLenght = 64
            },
            Symbol = new StringControl()
            {
                IsRequired = true,
                Value = currency.Symbol,
                MinLenght = 1,
                MaxLenght = 3
            },
            Description = new StringControl()
            {
                IsRequired = false,
                Value = currency.Description,
                MinLenght = 1,
                MaxLenght = 256
            },
            IsActive = new BoolControl()
            {
                IsRequired = true,
                Value = currency.IsActive
            }
        };

        return editForm;
    }
}