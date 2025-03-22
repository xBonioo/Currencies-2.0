using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Create;
using FluentValidation;

namespace Currencies.WebApi.Validators.UserCurrencyAmount;

public class CreateUserCurrencyAmountValidator : AbstractValidator<CreateUserCurrencyAmountCommand>
{
    public CreateUserCurrencyAmountValidator(UserCurrencyAmountDtoValidator usercurrencyamountValidator)
    {
        RuleFor(x => x.Data).SetValidator(usercurrencyamountValidator);
    }
}
