using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Create;

public class CreateUserCurrencyAmountCommandHandler : IRequestHandler<CreateUserCurrencyAmountCommand, UserCurrencyAmountDto?>
{
    private readonly IUserCurrencyAmountService _userCurrencyAmountService;

    public CreateUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    {
        _userCurrencyAmountService = userCurrencyAmountService;
    }

    public async Task<UserCurrencyAmountDto?> Handle(CreateUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await _userCurrencyAmountService.AddAsync(request.Data, cancellationToken);
    }
}
