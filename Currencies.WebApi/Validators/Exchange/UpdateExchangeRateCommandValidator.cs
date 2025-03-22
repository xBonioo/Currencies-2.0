using Currencies.Db;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Update;
using FluentValidation;

namespace Currencies.WebApi.Validators.Exchange;

public class UpdateExchangeRateCommandValidator : AbstractValidator<UpdateExchangeRateCommand>
{
    public UpdateExchangeRateCommandValidator(TableContext dbContext)
    {
        RuleFor(x => x.Dto.FromCurrencyId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.ToCurrencyId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.Rate)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.Direction)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.IsActive)
            .NotNull()
            .NotEmpty();
    }
}