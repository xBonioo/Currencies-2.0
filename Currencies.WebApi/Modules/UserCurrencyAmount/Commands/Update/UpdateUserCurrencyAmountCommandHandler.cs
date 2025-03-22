using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;

public class UpdateUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    : IRequestHandler<UpdateUserCurrencyAmountCommand, UserCurrencyAmountDto>
{
    public async Task<UserCurrencyAmountDto?> Handle(UpdateUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await userCurrencyAmountService.UpdateAsync(request.Id, request.Dto, cancellationToken);
    }
}
