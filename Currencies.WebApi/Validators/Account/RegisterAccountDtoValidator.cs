using Currencies.Application.ModelDtos.User;
using Currencies.Db;
using FluentValidation;

namespace Currencies.WebApi.Validators.Account;

public class RegisterAccountDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterAccountDtoValidator(TableContext dbContext)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotNull()
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .Matches("[a-z]").WithMessage("Password must have at least one lowercase ('a'-'z')")
            .Matches("[A-Z]").WithMessage("Password must have at least one uppercase ('A'-'Z')")
            .Matches(@"\d").WithMessage("Password must have at least one digit ('0'-'9')")
            .Matches(@"[\W]").WithMessage("Password must have at least one non alphanumeric character");

        RuleFor(x => x.ConfirmPassword).Equal(e => e.Password);

        RuleFor(x => x.Email)
            .Custom((value, context) =>
            {
                var emailInUse = dbContext.Users.Any(u => u.Email == value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "That email is taken");
                }
            });

        RuleFor(x => x.UserName)
            .NotNull()
            .MaximumLength(256)
            .Custom((value, context) =>
            {
                var isNameAlreadyTaken = dbContext.Users.Any(p => p.UserName == value);
                if (isNameAlreadyTaken)
                {
                    context.AddFailure("UserName", "This username is already taken");
                }
            });
    }
}
