using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Currency;
using Currencies.Db.Entities;

namespace Currencies.Application.Interfaces;

public interface ICurrencyService : IEntityService<Currency>
{
    Task<PageResult<CurrencyDto>?> GetAllCurrenciesAsync(FilterCurrencyDto filter, CancellationToken cancellationToken);
    Task<CurrencyDto?> CreateAsync(BaseCurrencyDto dto, CancellationToken cancellationToken);
    Task<CurrencyDto?> UpdateAsync(int id, BaseCurrencyDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}