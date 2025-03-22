using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using Currencies.Db.Entities;

namespace Currencies.Application.Interfaces;

public interface IUserExchangeHistoryService : IEntityService<UserExchangeHistory>
{
    Task<PageResult<UserExchangeHistoryDto>> GetAllUserExchangeHistoryServiceiesAsync(FilterUserExchangeHistoryDto filter, CancellationToken cancellationToken);
    Task<bool> AddUserExchangeHistoryAsync(UserExchangeHistoryDto dto, CancellationToken cancellationToken);
}