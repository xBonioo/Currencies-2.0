using Currencies.Application.ModelDtos.Role;
using Currencies.Db;
using FluentValidation;

namespace Currencies.WebApi.Validators.Role;

public class RoleDtoValidator : AbstractValidator<BaseRoleDto>
{
    public RoleDtoValidator(TableContext dbContext)
    {
        RuleFor(x => x.Name)
        .NotNull()
        .NotEmpty()
        .MaximumLength(64)
        .Custom((value, context) =>
        {
            var isNameAlreadyTaken = dbContext.Roles.Any(p => p.Name == value);
            if (isNameAlreadyTaken)
            {
                context.AddFailure("Name", "This role's name has been already taken");
            }
        });

        RuleFor(x => x.IsActive)
           .NotNull()
           .NotEmpty();
    }
}

