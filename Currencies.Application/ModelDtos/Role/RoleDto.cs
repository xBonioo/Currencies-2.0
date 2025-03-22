namespace Currencies.Application.ModelDtos.Role;

public class RoleDto : BaseRoleDto
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}