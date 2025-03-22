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

public class UserCurrencyAmountService : IUserCurrencyAmountService
{
    private readonly TableContext _dbContext;
    private readonly IMapper _mapper;

    public UserCurrencyAmountService(TableContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<UserCurrencyAmountDto?> ConvertAsync(ConvertUserCurrencyAmountDto dto, CancellationToken cancellationToken)
    {
        var accountFrom = await _dbContext.UserCurrencyAmounts
            .FirstOrDefaultAsync(x => x.CurrencyId == dto.FromCurrencyId && x.UserId == dto.UserId, cancellationToken);

        if (accountFrom == null)
        {
            throw new NotFoundException($"Client doesn't have account in currency id: {dto.FromCurrencyId}");
        }

        if(accountFrom.Amount - dto.Amount < 0)
        {
            throw new BadRequestException("Brak środków na koncie");
        }

        var exchangeRateTo = new ExchangeRate();
        int? rateId = null;
        if (dto.FromCurrencyId == 4)  // From PLN to foreign currency
        {
            exchangeRateTo = await _dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.FromCurrencyID == dto.FromCurrencyId && x.ToCurrencyID == dto.ToCurrencyId && x.Direction == Direction.Buy, cancellationToken);
            rateId = exchangeRateTo!.Id;
        }
        else if (dto.ToCurrencyId == 4)  // From foreign currency to PLN
        {
            exchangeRateTo = await _dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.ToCurrencyID == dto.FromCurrencyId && x.FromCurrencyID == dto.ToCurrencyId && x.Direction == Direction.Sell, cancellationToken);
            rateId = exchangeRateTo!.Id;
        }
        else  // From one foreign currency to another via PLN
        {
            // Convert from the starting currency to PLN
            var rateToPLN = await _dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.FromCurrencyID == 4 && x.ToCurrencyID == dto.FromCurrencyId && x.Direction == Direction.Sell, cancellationToken);

            // Convert from PLN to the target currency
            var rateFromPLN = await _dbContext.ExchangeRate
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync(x => x.FromCurrencyID == 4 && x.ToCurrencyID == dto.ToCurrencyId && x.Direction == Direction.Buy, cancellationToken);

            if (rateToPLN != null && rateFromPLN != null)
            {
                exchangeRateTo = new ExchangeRate()
                {
                    Rate = (1 / rateFromPLN.Rate) * rateToPLN.Rate
                };
            }
            else
            {
                throw new NotFoundException("Cannot find appropriate exchange rates to perform conversion.");
            }
        }

        decimal convertedAmount = dto.Amount * exchangeRateTo.Rate;

        var accountTo = await _dbContext.UserCurrencyAmounts
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
            await _dbContext.UserCurrencyAmounts.AddAsync(currencyAmount, cancellationToken);
            result = _mapper.Map<UserCurrencyAmountDto>(currencyAmount);
            result.RateId = rateId;
        }
        else
        {
            accountTo.Amount += convertedAmount;
            result = _mapper.Map<UserCurrencyAmountDto>(accountTo);
            result.RateId = rateId;
        }

        accountFrom.Amount -= dto.Amount;

        if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return result;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(ConvertAsync)}");
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var user_currency_amount = await GetByIdAsync(id, cancellationToken);
        if (user_currency_amount == null || !user_currency_amount.IsActive)
        {
            throw new NotFoundException("Currency not found");
        }

        user_currency_amount.IsActive = false;

        if ((await _dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(DeleteAsync)}");
    }

    public async Task<PageResult<UserCurrencyAmountDto>?> GetAllUserCurrencyAmountsAsync(FilterUserCurrencyAmountDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext
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
            .ProjectTo<UserCurrencyAmountDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<UserCurrencyAmountDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<UserCurrencyAmountDto?> AddAsync(BaseUserCurrencyAmountDto dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            return null;
        }

        var user_currency_amount = await _dbContext.UserCurrencyAmounts
                        .FirstOrDefaultAsync(x => x.CurrencyId == dto.CurrencyId && x.UserId == dto.UserId && x.IsActive, cancellationToken);
        if (user_currency_amount != null)
        {
            user_currency_amount.Amount += dto.Amount;
        }
        else
        {
            user_currency_amount = new UserCurrencyAmount()
            {
                UserId = dto.UserId,
                CurrencyId = dto.CurrencyId,
                Amount = dto.Amount
            };

            await _dbContext.UserCurrencyAmounts.AddAsync(user_currency_amount, cancellationToken);
        }

        if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return _mapper.Map<UserCurrencyAmountDto>(user_currency_amount);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(AddAsync)}");
    }

    public async Task<UserCurrencyAmountDto?> UpdateAsync(int id, BaseUserCurrencyAmountDto dto, CancellationToken cancellationToken)
    {
        var user_currency_amount = await GetByIdAsync(id, cancellationToken);
        if (user_currency_amount == null || !user_currency_amount.IsActive)
        {
            throw new NotFoundException("User currency account not found");
        }

        user_currency_amount.UserId = dto.UserId;
        user_currency_amount.CurrencyId = dto.CurrencyId;
        user_currency_amount.Amount += dto.Amount;
        user_currency_amount.IsActive = dto.IsActive;

        if ((await _dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return _mapper.Map<UserCurrencyAmountDto>(user_currency_amount);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(UpdateAsync)}");
    }

    public async Task<List<UserCurrencyAmount>> GetByUserIdAsync(string id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
           .UserCurrencyAmounts
           .Where(x => x.UserId == id)
           .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<UserCurrencyAmount?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
           .UserCurrencyAmounts
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result;
    }
}