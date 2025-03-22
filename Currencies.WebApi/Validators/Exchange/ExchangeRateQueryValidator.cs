using Currencies.Abstractions.Contracts.Helpers;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetAll;
using FluentValidation;

namespace Currencies.WebApi.Validators.Exchange;

public class ExchangeRateQueryValidator : AbstractValidator<GetExchangeRatesListQuery>
{
    public ExchangeRateQueryValidator()
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