using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using Currencies.Db.Entities;

namespace Currencies.Application.Interfaces;

public interface IUserCurrencyAmountService : IEntityService<UserCurrencyAmount>
{
    Task<PageResult<UserCurrencyAmountDto>> GetAllUserCurrencyAmountsAsync(FilterUserCurrencyAmountDto filter, CancellationToken cancellationToken);
    Task<UserCurrencyAmountDto?> ConvertAsync(ConvertUserCurrencyAmountDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<UserCurrencyAmountDto?> AddAsync(BaseUserCurrencyAmountDto? dto, CancellationToken cancellationToken);
    Task<UserCurrencyAmountDto?> UpdateAsync(int id, BaseUserCurrencyAmountDto dto, CancellationToken cancellationToken);
    Task<List<UserCurrencyAmount>> GetByUserIdAsync(string id, CancellationToken cancellationToken);
}