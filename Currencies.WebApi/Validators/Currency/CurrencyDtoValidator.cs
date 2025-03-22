using Currencies.Application.ModelDtos.Currency;
using Currencies.Db;
using FluentValidation;

namespace Currencies.WebApi.Validators.Currency;

public class CurrencyDtoValidator : AbstractValidator<BaseCurrencyDto>
{
    public CurrencyDtoValidator(TableContext dbContext)
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(64)
            .Custom((value, context) =>
            {
                var isNameAlreadyTaken = dbContext.Currencies.Any(p => p.Name == value);
                if (isNameAlreadyTaken)
                {
                    context.AddFailure("Name", "This currency's name has been already taken");
                }
            });


        RuleFor(x => x.Symbol)
            .NotNull()
            .NotEmpty()
            .MaximumLength(3)
            .Custom((value, context) =>
            {
                var isAliasAlreadyTaken = dbContext.Currencies.Any(p => p.Symbol == value);
                if (isAliasAlreadyTaken)
                {
                    context.AddFailure("Symbol", "This currency's symbol has been already taken");
                }
            });

        RuleFor(x => x.IsActive)
            .NotNull()
            .NotEmpty();
    }
}