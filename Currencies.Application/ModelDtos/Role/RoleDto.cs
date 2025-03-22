namespace Currencies.Application.ModelDtos.Role;

public sealed class RoleDto : BaseRoleDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}