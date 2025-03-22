using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Controls;
using Currencies.Abstractions.Forms;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetEditForm;

public class GetExchangeRateEditFormQueryHandler(IExchangeRateService exchangeRateService)
    : IRequestHandler<GetExchangeRateEditFormQuery, ExchangeRateEditForm?>
{
    public async Task<ExchangeRateEditForm?> Handle(GetExchangeRateEditFormQuery request, CancellationToken cancellationToken)
    {
        var exchangeRate = await exchangeRateService.GetByIdAsync(request.id, cancellationToken);
        if (exchangeRate is not { IsActive: true })
        {
            var createForm = new ExchangeRateEditForm()
            {
                FromCurrencyId = new IntegerControl()
                {
                    IsRequired = true,
                    Value = 0,
                    MinValue = 1,
                    MaxValue = 15
                },
                ToCurrencyId = new IntegerControl()
                {
                    IsRequired = true,
                    Value = 0,
                    MinValue = 1,
                    MaxValue = 15
                },
                Rate = new DecimalControl()
                {
                    IsRequired = true,
                    Value = 0,
                    MinValue = 0.1m,
                    MaxValue = 24
                },
                Direction = new EnumControl<Direction>()
                {
                    IsRequired = true,
                    Value = null
                },
                IsActive = new BoolControl()
                {
                    IsRequired = true,
                    Value = true
                }
            };

            return createForm;
        }

        var editForm = new ExchangeRateEditForm()
        {
            FromCurrencyId = new IntegerControl()
            {
                IsRequired = true,
                Value = exchangeRate.FromCurrencyId,
                MinValue = 1,
                MaxValue = 15
            },
            ToCurrencyId = new IntegerControl()
            {
                IsRequired = true,
                Value = exchangeRate.ToCurrencyId,
                MinValue = 1,
                MaxValue = 15
            },
            Rate = new DecimalControl()
            {
                IsRequired = true,
                Value = exchangeRate.Rate,
                MinValue = 0.1m,
                MaxValue = 24
            },
            Direction = new EnumControl<Direction>()
            {
                IsRequired = true,
                Value = exchangeRate.Direction
            },
            IsActive = new BoolControl()
            {
                IsRequired = true,
                Value = exchangeRate.IsActive
            }
        };

        return editForm;
    }
}