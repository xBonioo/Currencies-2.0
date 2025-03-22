using Currencies.Db;
using Currencies.WebApi.Modules.Role.Commands.Update;
using FluentValidation;

namespace Currencies.WebApi.Validators.Role;


public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(TableContext dbContext)
    {
        RuleFor(x => x.Dto.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(64)
            .Custom((value, context) =>
            {
                var editedCurrency = context.InstanceToValidate;
                var isNameAlreadyTaken = dbContext.Roles.Any(p => p.Name == value && p.Id != editedCurrency.Id);
                if (isNameAlreadyTaken)
                {
                    context.AddFailure("Name", "This role's name has been already taken");
                }
            });

        RuleFor(x => x.Dto.IsActive)
            .NotNull()
            .NotEmpty();
    }

}
