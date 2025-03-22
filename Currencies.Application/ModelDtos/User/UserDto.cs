namespace Currencies.Application.ModelDtos.User;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string Adres { get; set; } = null!;
    public int IdentityNumber { get; set; }
    public string IDNumber { get; set; } = null!;
    public DateTime IDExpiryDate { get; set; }
    public DateTime IDIssueDate { get; set; }
    public bool IsActive { get; set; }
}
