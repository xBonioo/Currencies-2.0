using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Delete;

public class DeleteUserCurrencyAmountCommandHandler(IUserCurrencyAmountService userCurrencyAmountService)
    : IRequestHandler<DeleteUserCurrencyAmountCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCurrencyAmountCommand request, CancellationToken cancellationToken)
    {
        return await userCurrencyAmountService.DeleteAsync(request.Id, cancellationToken);
    }
}
