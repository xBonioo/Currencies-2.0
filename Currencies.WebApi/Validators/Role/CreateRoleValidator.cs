using Currencies.WebApi.Modules.Role.Commands.Create;
using FluentValidation;

namespace Currencies.WebApi.Validators.Role;

public class CreateRoleValidator: AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator(RoleDtoValidator roleValidator)
    {
        RuleFor(x => x.Data).SetValidator(roleValidator);
    }
}
