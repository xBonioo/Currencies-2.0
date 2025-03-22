using Currencies.Abstractions.Controls;
using Currencies.Abstractions.Forms;
using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.Role.Queries.GetEditForm;

public class GetRoleEditFormQueryHandler(IRoleService roleService)
    : IRequestHandler<GetRoleEditFormQuery, RoleEditForm?>
{
    public async Task<RoleEditForm?> Handle(GetRoleEditFormQuery request, CancellationToken cancellationToken)
    {
        var role = await roleService.GetByIdAsync(request.id, cancellationToken);
        if (role is not { IsActive: true })
        {
            var createForm = new RoleEditForm()
            {
                Name = new StringControl()
                {
                    IsRequired = true,
                    Value = string.Empty,
                    MinLenght = 1,
                    MaxLenght = 64
                },
                IsActive = new BoolControl()
                {
                    IsRequired = true,
                    Value = true
                }
            };

            return createForm;
        }

        var editForm = new RoleEditForm()
        {
            Name = new StringControl()
            {
                IsRequired = true,
                Value = role.Name,
                MinLenght = 1,
                MaxLenght = 64
            },
            IsActive = new BoolControl()
            {
                IsRequired = true,
                Value = role.IsActive
            }
        };

        return editForm;
    }
}