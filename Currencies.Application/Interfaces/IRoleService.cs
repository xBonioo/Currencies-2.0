using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Role;
using Currencies.Db.Entities;

namespace Currencies.Application.Interfaces;

public interface IRoleService : IEntityService<Role>
{
    Task<PageResult<RoleDto>> GetAllRolesAsync(FilterRoleDto filter, CancellationToken cancellationToken);
    Task<RoleDto?> CreateAsync(BaseRoleDto? dto, CancellationToken cancellationToken);
    Task<RoleDto?> UpdateAsync(int id, BaseRoleDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}