using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Create;

public class CreateUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    : IRequestHandler<CreateUserCurrencyAmountCommand, UserCurrencyAmountDto?>
{
    public async Task<UserCurrencyAmountDto?> Handle(CreateUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await userCurrencyAmountService.AddAsync(request.Data, cancellationToken);
    }
}
