using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;

public class UpdateUserCurrencyAmountCommandHandler : IRequestHandler<UpdateUserCurrencyAmountCommand, UserCurrencyAmountDto>
{
    private readonly IUserCurrencyAmountService _userCurrencyAmountService;

    public UpdateUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    {
        _userCurrencyAmountService = userCurrencyAmountService;
    }

    public async Task<UserCurrencyAmountDto?> Handle(UpdateUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await _userCurrencyAmountService.UpdateAsync(request.Id, request.Dto, cancellationToken);
    }
}
