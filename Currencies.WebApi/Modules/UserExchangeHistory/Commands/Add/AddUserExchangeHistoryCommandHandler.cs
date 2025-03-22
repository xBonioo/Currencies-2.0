using Currencies.Application.Interfaces;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Commands.Add;

public class AddUserExchangeHistoryCommandHandler(IUserExchangeHistoryService userExchangeHistoryService)
    : IRequestHandler<AddUserExchangeHistoryCommand, bool>
{
    public async Task<bool> Handle(AddUserExchangeHistoryCommand request, CancellationToken cancellationToken)
    {
        return await userExchangeHistoryService.AddUserExchangeHistoryAsync(request.Data, cancellationToken);
    }
}
