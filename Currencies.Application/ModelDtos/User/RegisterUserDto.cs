namespace Currencies.Application.ModelDtos.User;

public sealed class RegisterUserDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Adres { get; set; } = null!;
    public int IdentityNumber { get; set; }
    public string IdNumber { get; set; } = null!;
    public DateTime IdExpiryDate { get; set; }
    public DateTime IdIssueDate { get; set; }
}
