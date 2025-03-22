using Currencies.Db;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Create;
using FluentValidation;

namespace Currencies.WebApi.Validators.Exchange;

public class CreateExchangeRateValidator : AbstractValidator<CreateExchangeRateCommand>
{
    public CreateExchangeRateValidator(TableContext dbContext)
    {
        RuleFor(x => x.Date)
            .NotNull()
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Today.AddDays(-3)).WithMessage("Date cannot be in the past.")
            .LessThan(DateTime.Today.AddYears(1)).WithMessage("Date cannot be more than one year in the future.");
    }
}