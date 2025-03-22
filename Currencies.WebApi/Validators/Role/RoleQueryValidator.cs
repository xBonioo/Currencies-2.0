using Currencies.Abstractions.Contracts.Helpers;
using Currencies.WebApi.Modules.Role.Queries.GetAll;
using FluentValidation;

namespace Currencies.WebApi.Validators.Role;

public class RoleQueryValidator : AbstractValidator<GetRolesListQuery>
{
    public RoleQueryValidator()
    {
        RuleFor(r => r.Filter.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.Filter.PageSize).Custom((value, context) =>
        {
            if (!PropertyForQuery.AllowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must in [{string.Join(", ", PropertyForQuery.AllowedPageSizes)}]");
            }
        });
    }
}
