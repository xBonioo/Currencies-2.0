using AutoMapper;
using AutoMapper.QueryableExtensions;
using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using Currencies.Db;
using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Application.Services;

public class UserCurrencyAmountService(TableContext dbContext, IMapper mapper) : IUserCurrencyAmountService
{
    public async Task<UserCurrencyAmountDto?> ConvertAsync(ConvertUserCurrencyAmountDto dto, CancellationToken cancellationToken)
    {
        var accountFrom = await dbContext.UserCurrencyAmounts
            .FirstOrDefaultAsync(x => x.CurrencyId == dto.FromCurrencyId && x.UserId == dto.UserId, cancellationToken);

        if (accountFrom == null)
        {
            throw new NotFoundException($"Client doesn't have account in currency id: {dto.FromCurrencyId}");
        }

        if(accountFrom.Amount - dto.Amount < 0)
        {
            throw new BadRequestException("Brak środków na koncie");
        }

        ExchangeRate? exchangeRateTo;
        int? rateId = null;
        if (dto.FromCurrencyId == 4)  // From PLN to foreign currency
        {
            exchangeRateTo = await dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.FromCurrencyId == dto.FromCurrencyId && x.ToCurrencyId == dto.ToCurrencyId && x.Direction == Direction.Buy, cancellationToken);
            rateId = exchangeRateTo!.Id;
        }
        else if (dto.ToCurrencyId == 4)  // From foreign currency to PLN
        {
            exchangeRateTo = await dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.ToCurrencyId == dto.FromCurrencyId && x.FromCurrencyId == dto.ToCurrencyId && x.Direction == Direction.Sell, cancellationToken);
            rateId = exchangeRateTo!.Id;
        }
        else  // From one foreign currency to another via PLN
        {
            // Convert from the starting currency to PLN
            var rateToPln = await dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.FromCurrencyId == 4 && x.ToCurrencyId == dto.FromCurrencyId && x.Direction == Direction.Sell, cancellationToken);

            // Convert from PLN to the target currency
            var rateFromPln = await dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.FromCurrencyId == 4 && x.ToCurrencyId == dto.ToCurrencyId && x.Direction == Direction.Buy, cancellationToken);

            if (rateToPln != null && rateFromPln != null)
            {
                exchangeRateTo = new ExchangeRate()
                {
                    Rate = (1 / rateFromPln.Rate) * rateToPln.Rate
                };
            }
            else
            {
                throw new NotFoundException("Cannot find appropriate exchange rates to perform conversion.");
            }
        }

        var convertedAmount = dto.Amount * exchangeRateTo.Rate;

        var accountTo = await dbContext.UserCurrencyAmounts
            .FirstOrDefaultAsync(x => x.CurrencyId == dto.ToCurrencyId && x.UserId == dto.UserId, cancellationToken);

        UserCurrencyAmountDto result;
        if (accountTo == null)
        {
            var currencyAmount = new UserCurrencyAmount()
            {
                CurrencyId = dto.ToCurrencyId,
                Amount = convertedAmount,
                UserId = dto.UserId
            };
            await dbContext.UserCurrencyAmounts.AddAsync(currencyAmount, cancellationToken);
            result = mapper.Map<UserCurrencyAmountDto>(currencyAmount);
            result.RateId = rateId;
        }
        else
        {
            accountTo.Amount += convertedAmount;
            result = mapper.Map<UserCurrencyAmountDto>(accountTo);
            result.RateId = rateId;
        }

        accountFrom.Amount -= dto.Amount;

        if (await dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return result;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(ConvertAsync)}");
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var userCurrencyAmount = await GetByIdAsync(id, cancellationToken);
        if (userCurrencyAmount is not { IsActive: true })
        {
            throw new NotFoundException("Currency not found");
        }

        userCurrencyAmount.IsActive = false;

        if ((await dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(DeleteAsync)}");
    }

    public async Task<PageResult<UserCurrencyAmountDto>> GetAllUserCurrencyAmountsAsync(FilterUserCurrencyAmountDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = dbContext
          .UserCurrencyAmounts
          .AsQueryable();

        if (!baseQuery.Any())
        {
            throw new NotFoundException("User currency accounts not found");
        }

        if (!string.IsNullOrEmpty(filter.SearchPhrase))
        {
            baseQuery = baseQuery.Where(x => x.UserId.Contains(filter.SearchPhrase));
        }
        if (filter.IsActive != null)
        {
            baseQuery = baseQuery.Where(x => x.IsActive == filter.IsActive);
        }

        var totalItemCount = baseQuery.Count();

        var itemsDto = await baseQuery
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ProjectTo<UserCurrencyAmountDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<UserCurrencyAmountDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<UserCurrencyAmountDto?> AddAsync(BaseUserCurrencyAmountDto? dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            return null;
        }

        var userCurrencyAmount = await dbContext.UserCurrencyAmounts
                        .FirstOrDefaultAsync(x => x.CurrencyId == dto.CurrencyId && x.UserId == dto.UserId && x.IsActive, cancellationToken);
        if (userCurrencyAmount != null)
        {
            userCurrencyAmount.Amount += dto.Amount;
        }
        else
        {
            userCurrencyAmount = new UserCurrencyAmount()
            {
                UserId = dto.UserId,
                CurrencyId = dto.CurrencyId,
                Amount = dto.Amount
            };

            await dbContext.UserCurrencyAmounts.AddAsync(userCurrencyAmount, cancellationToken);
        }

        if (await dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return mapper.Map<UserCurrencyAmountDto>(userCurrencyAmount);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(AddAsync)}");
    }

    public async Task<UserCurrencyAmountDto?> UpdateAsync(int id, BaseUserCurrencyAmountDto dto, CancellationToken cancellationToken)
    {
        var userCurrencyAmount = await GetByIdAsync(id, cancellationToken);
        if (userCurrencyAmount is not { IsActive: true })
        {
            throw new NotFoundException("User currency account not found");
        }

        userCurrencyAmount.UserId = dto.UserId;
        userCurrencyAmount.CurrencyId = dto.CurrencyId;
        userCurrencyAmount.Amount += dto.Amount;
        userCurrencyAmount.IsActive = dto.IsActive;

        if ((await dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return mapper.Map<UserCurrencyAmountDto>(userCurrencyAmount);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(UpdateAsync)}");
    }

    public async Task<List<UserCurrencyAmount>> GetByUserIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await dbContext
           .UserCurrencyAmounts
           .Where(x => x.UserId == id)
           .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<UserCurrencyAmount?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext
           .UserCurrencyAmounts
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result;
    }
}