using AutoMapper;
using AutoMapper.QueryableExtensions;
using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.Db;
using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Application.Services;

public class ExchangeRateService(TableContext dbContext, IMapper mapper) : IExchangeRateService
{
    public async Task<List<ExchangeRateDto?>> CreateAsync(List<CurrencyExchangeRateDto> currencyExchangeRateList, CancellationToken cancellationToken)
    {
        if (currencyExchangeRateList == null || currencyExchangeRateList.Count == 0)
        {
            throw new NotFoundException("Exchange rates not found");
        }


        var activeRates = dbContext.ExchangeRate.Where(x => x.IsActive);
        foreach (var rate in activeRates)
        {
            rate.IsActive = false;
        }

        var currencies = await dbContext.Currencies
            .AsQueryable()
            .ToListAsync(cancellationToken);

        var result = new List<ExchangeRate>();
        foreach (var item in currencyExchangeRateList)
        {
            var buyRate = new ExchangeRate
            {
                Rate = item.Ask,
                FromCurrencyId = 4,
                ToCurrencyId = currencies.FirstOrDefault(x => x.Symbol.Contains(item.Code))!.Id,
                Direction = Direction.Buy
            };
            result.Add(buyRate);

            var sellRate = new ExchangeRate
            {
                Rate = item.Bid,
                FromCurrencyId = 4,
                ToCurrencyId = currencies.FirstOrDefault(x => x.Symbol.Contains(item.Code))!.Id,
                Direction = Direction.Sell
            };
            result.Add(sellRate);
        }

        if (!result.Any())
        {
            throw new NotFoundException("Exchange rates not found");
        }

        await dbContext.ExchangeRate.AddRangeAsync(result, cancellationToken);

        if (await dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return mapper.Map<List<ExchangeRateDto?>>(result);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(CreateAsync)}");
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var exchangeRate = await GetByIdAsync(id, cancellationToken);
        if (exchangeRate == null)
        {
            throw new NotFoundException("Exchange rate not found");
        }

        exchangeRate.IsActive = false;

        if ((await dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(DeleteAsync)}");
    }

    public async Task<PageResult<ExchangeRateDto>> GetAllExchangeRateAsync(FilterExchangeRateDto filter,CancellationToken cancellationToken)
    {
        var baseQuery = dbContext
            .ExchangeRate
            .AsQueryable()
            .Include(x => x.FromCurrency)
            .Include(x => x.ToCurrency);

        var totalItemCount = baseQuery.Count();

        var itemsDto = await baseQuery
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ProjectTo<ExchangeRateDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<ExchangeRateDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<ExchangeRateDto?> UpdateAsync(int id, BaseExchangeRateDto dto, CancellationToken cancellationToken)
    {
        var exchangeRate = await GetByIdAsync(id, cancellationToken);
        if (exchangeRate == null)
        {
            throw new NotFoundException("Exchange rate not found");
        }

        exchangeRate.FromCurrencyId = dto.FromCurrencyId;
        exchangeRate.ToCurrencyId = dto.ToCurrencyId;
        exchangeRate.Rate = dto.Rate;
        exchangeRate.Direction = dto.Direction;
        exchangeRate.IsActive = dto.IsActive;

        if ((await dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return mapper.Map<ExchangeRateDto>(exchangeRate);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(UpdateAsync)}");
    }

    public async Task<(ExchangeRate?, ExchangeRate?)> GetByIdFromCurrencyAsync(int fromId, int toId, CancellationToken cancellationToken)
    {
        var exchangeRates = await dbContext
                            .ExchangeRate
                            .AsQueryable()
                            .Include(x => x.FromCurrency)
                            .Include(x => x.ToCurrency)
                            .Where(x => x.FromCurrencyId == fromId && x.ToCurrencyId == toId)
                            .OrderByDescending(x => x.CreatedOn)
                            .ToListAsync(cancellationToken);

        if (exchangeRates == null)
        {
            throw new NotFoundException("Exchange rate not found");
        }

        var direction0Rate = exchangeRates.FirstOrDefault(x => x.Direction == Direction.Buy);
        var direction1Rate = exchangeRates.FirstOrDefault(x => x.Direction == Direction.Sell);

        var result = (Direction0: direction0Rate, Direction1: direction1Rate);

        return result;
    }

    public async Task<List<ExchangeRate>> GetByCurrencyIdAsync(int id, int direction, CancellationToken cancellationToken)
    {
        return await dbContext
            .ExchangeRate
            .AsQueryable()
            .Include(x => x.FromCurrency)
            .Include(x => x.ToCurrency)
            .Where(x => x.ToCurrencyId == id && (int)x.Direction == direction)
            .ToListAsync(cancellationToken);
    }

    public async Task<ExchangeRate?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
       return await dbContext
                    .ExchangeRate
                    .AsQueryable()
                    .Include(x => x.FromCurrency)
                    .Include(x => x.ToCurrency)
                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}