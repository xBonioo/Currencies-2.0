using AutoMapper;
using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Abstractions.Contracts.Helpers;
using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Currency;
using Currencies.Db;
using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Application.Services;

public class CurrencyService(TableContext dbContext, IMapper mapper) : ICurrencyService
{
    public async Task<CurrencyDto?> CreateAsync(BaseCurrencyDto? dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            return null;
        }

        var currency = new Currency()
        {
            Name = dto.Name,
            Symbol = dto.Symbol,
            Description = dto.Description
        };

        dbContext.Currencies.Add(currency);

        if (await dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return mapper.Map<CurrencyDto>(currency);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(CreateAsync)}");
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var currency = await GetByIdAsync(id, cancellationToken);
        if (currency == null || !currency.IsActive)
        {
            throw new NotFoundException("Currencies not found");
        }

        currency.IsActive = false;

        if ((await dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(DeleteAsync)}");
    }

    public async Task<PageResult<CurrencyDto>?> GetAllCurrenciesAsync(FilterCurrencyDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = from c in dbContext.Currencies
                    join er0 in dbContext.ExchangeRate.Where(er => er.Direction == Direction.Buy && er.IsActive)
                        on c.Id equals er0.ToCurrencyID into er0Group
                    from er0 in er0Group.DefaultIfEmpty()
                    join er1 in dbContext.ExchangeRate.Where(er => er.Direction == Direction.Sell && er.IsActive)
                        on c.Id equals er1.ToCurrencyID into er1Group
                    from er1 in er1Group.DefaultIfEmpty()
                    where c.IsActive
                    select new AnonymousTypeModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Symbol = c.Symbol,
                        Description = c.Description,
                        Rate_Direction_0 = er0 != null ? er0.Rate : (decimal?)null,
                        Rate_Direction_1 = er1 != null ? er1.Rate : (decimal?)null,
                        IsActive = c.IsActive
                    };


        if (!baseQuery.Any())
        {
            throw new NotFoundException("Currency not found");
        }

        if (!string.IsNullOrEmpty(filter.SearchPhrase))
        {
            baseQuery = baseQuery.Where(x => x.Name.Contains(filter.SearchPhrase) || x.Symbol.Contains(filter.SearchPhrase) || (!string.IsNullOrEmpty(x.Description) && x.Description.Contains(filter.SearchPhrase)));
        }
        if (filter.IsActive != null)
        {
            baseQuery = baseQuery.Where(x => x.IsActive == filter.IsActive);
        }

        var totalItemCount = baseQuery.Count();

        var itemsDto = baseQuery
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .AsEnumerable()
            .Select(x => mapper.Map<CurrencyDto>(x))
            .ToList();

        return new PageResult<CurrencyDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<CurrencyDto?> UpdateAsync(int id, BaseCurrencyDto dto, CancellationToken cancellationToken)
    {
        var currency = await GetByIdAsync(id, cancellationToken);
        if (currency == null || !currency.IsActive)
        {
            throw new NotFoundException("Currency not found");
        }

        currency.Name = dto.Name;
        currency.Symbol = dto.Symbol;
        currency.Description = dto.Description;
        currency.IsActive = dto.IsActive;

        if ((await dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return mapper.Map<CurrencyDto>(currency);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(UpdateAsync)}");
    }

    public async Task<Currency?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext
            .Currencies
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result;
    }
}