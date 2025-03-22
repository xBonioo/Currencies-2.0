using AutoMapper;
using AutoMapper.QueryableExtensions;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using Currencies.Db;
using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Application.Services;

public class UserExchangeHistoryService(TableContext dbContext, IMapper mapper) : IUserExchangeHistoryService
{
    public async Task<bool> AddUserExchangeHistoryAsync(UserExchangeHistoryDto? dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            return false;
        }

        var history = new UserExchangeHistory()
        {
            UserId = dto.UserId,
            RateId = dto.RateId,
            Amount = dto.Amount,
            AccountId = dto.AccountId,
            PaymentStatus = dto.PaymentStatus,
            PaymentType = dto.PaymentType,
        };

        dbContext.UserExchangeHistories.Add(history);

        if (await dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(AddUserExchangeHistoryAsync)}");
    }

    public async Task<PageResult<UserExchangeHistoryDto>> GetAllUserExchangeHistoryServiceiesAsync(FilterUserExchangeHistoryDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = dbContext
         .UserExchangeHistories
         .AsQueryable();

        if (!baseQuery.Any())
        {
            throw new NotFoundException("User exchange history not found");
        }

        if (!string.IsNullOrEmpty(filter.SearchPhrase))
        {
            baseQuery = baseQuery.Where(x => x.UserId.Contains(filter.SearchPhrase));
        }

        var totalItemCount = baseQuery.Count();

        var itemsDto = await baseQuery
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ProjectTo<UserExchangeHistoryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<UserExchangeHistoryDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<UserExchangeHistory?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await dbContext
           .UserExchangeHistories
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result;
    }
}