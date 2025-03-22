using Currencies.Abstractions.Contracts.Helpers;
using Currencies.WebApi.Modules.Currency.Queries.GetAll;
using FluentValidation;

namespace Currencies.WebApi.Validators.Currency;

public class CurrencyQueryValidator : AbstractValidator<GetCurrenciesListQuery>
{
    public CurrencyQueryValidator()
    {
        RuleFor(r => r.Filter.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.Filter.PageSize).Custom((value, context) =>
        {
            if (!PropertyForQuery.AllowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must in [{string.Join(", ", PropertyForQuery.AllowedPageSizes)}]");
            }
        });
    }
}