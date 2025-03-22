namespace Currencies.Application.ModelDtos.User;

public class RegisterUserDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Adres { get; set; } = null!;
    public int IdentityNumber { get; set; }
    public string IDNumber { get; set; } = null!;
    public DateTime IDExpiryDate { get; set; }
    public DateTime IDIssueDate { get; set; }
}
