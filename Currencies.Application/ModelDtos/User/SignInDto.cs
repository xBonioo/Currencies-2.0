namespace Currencies.Application.ModelDtos.User;

public sealed class SignInDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
