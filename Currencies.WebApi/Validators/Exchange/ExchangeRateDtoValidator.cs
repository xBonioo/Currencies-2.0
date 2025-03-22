using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.Db;
using FluentValidation;

namespace Currencies.WebApi.Validators.Exchange;

public class ExchangeRateDtoValidator : AbstractValidator<BaseExchangeRateDto>
{
    public ExchangeRateDtoValidator(TableContext dbContext)
    {
        RuleFor(x => x.FromCurrencyId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.ToCurrencyId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Rate)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.IsActive)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Direction)
            .NotNull()
            .NotEmpty();
    }
}