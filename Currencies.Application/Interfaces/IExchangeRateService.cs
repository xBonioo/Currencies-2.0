using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.Db.Entities;

namespace Currencies.Application.Interfaces;

public interface IExchangeRateService : IEntityService<ExchangeRate>
{
    Task<PageResult<ExchangeRateDto>> GetAllExchangeRateAsync(FilterExchangeRateDto filter,CancellationToken cancellationToken);
    Task<List<ExchangeRateDto?>> CreateAsync(List<CurrencyExchangeRateDto> currencyExchangeRateList, CancellationToken cancellationToken);
    Task<ExchangeRateDto?> UpdateAsync(int id, BaseExchangeRateDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<(ExchangeRate?, ExchangeRate?)> GetByIdFromCurrencyAsync(int fromId, int toId, CancellationToken cancellationToken);
    Task<List<ExchangeRate>> GetByCurrencyIdAsync(int id, int direction, CancellationToken cancellationToken);
}