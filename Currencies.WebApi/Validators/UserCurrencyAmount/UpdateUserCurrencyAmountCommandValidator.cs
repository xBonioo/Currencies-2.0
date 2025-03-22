using Currencies.Db;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;
using FluentValidation;

namespace Currencies.WebApi.Validators.UserCurrencyAmount;

public class UpdateUserCurrencyAmountCommandValidator : AbstractValidator<UpdateUserCurrencyAmountCommand>
{
    public UpdateUserCurrencyAmountCommandValidator(TableContext dbContext)
    {
        RuleFor(x => x.Dto.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.CurrencyId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.Amount)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.Dto.IsActive)
            .NotNull()
            .NotEmpty();
    }
}
