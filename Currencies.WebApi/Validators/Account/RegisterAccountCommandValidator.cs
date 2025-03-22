using Currencies.WebApi.Modules.User.Commands.Register;
using FluentValidation;

namespace Currencies.WebApi.Validators.Account;

public class RegisterAccountCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterAccountCommandValidator(RegisterAccountDtoValidator validator)
    {
        RuleFor(x => x.RegisterUserDto).SetValidator(validator);
    }
}