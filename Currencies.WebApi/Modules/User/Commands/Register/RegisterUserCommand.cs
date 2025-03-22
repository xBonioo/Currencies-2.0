using Currencies.Application.ModelDtos.User;
using MediatR;

namespace Currencies.WebApi.Modules.User.Commands.Register;

public class RegisterUserCommand : IRequest<UserDto>
{
    public RegisterUserDto RegisterUserDto { get; set; } = null!;
}