using Currencies.WebApi.Modules.Currency.Commands.Create;
using FluentValidation;

namespace Currencies.WebApi.Validators.Currency;

public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyValidator(CurrencyDtoValidator currencyValidator)
    {
        RuleFor(x => x.Data).SetValidator(currencyValidator);
    }
}