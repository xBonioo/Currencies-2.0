using Currencies.Application.ModelDtos.User.CurrencyAmount;
using Currencies.Db;
using FluentValidation;

namespace Currencies.WebApi.Validators.UserCurrencyAmount;

public class UserCurrencyAmountDtoValidator : AbstractValidator<BaseUserCurrencyAmountDto>
{
    public UserCurrencyAmountDtoValidator(TableContext dbContext)
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.CurrencyId)
            .NotNull()
            .NotEmpty()
            .Custom(async (currencyId, context) =>
            {
                var dto = context.InstanceToValidate as BaseUserCurrencyAmountDto;
                if (dto != null)
                {
                    //bool exists = dbContext.UserCurrencyAmounts
                    //    .Any(x => x.CurrencyId == dto.CurrencyId && x.UserId == dto.UserId && x.IsActive);
                    //if (exists)
                    //{
                    //    context.AddFailure("CurrencyId", "This user already has a currency account with the specified currency ID.");
                    //}

                    bool existsCurrency = dbContext.Currencies
                        .Any(x => x.Id == dto.CurrencyId && x.IsActive);
                    if (!existsCurrency)
                    {
                        context.AddFailure("CurrencyId", "CurrencyId doen't exist.");
                    }
                }
            });

        RuleFor(x => x.Amount)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.IsActive)
            .NotNull()
            .NotEmpty();
    }
}
