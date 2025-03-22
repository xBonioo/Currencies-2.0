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

public class UserExchangeHistoryService : IUserExchangeHistoryService
{
    private readonly TableContext _dbContext;
    private readonly IMapper _mapper;

    public UserExchangeHistoryService(TableContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<bool> AddUserExchangeHistoryAsync(UserExchangeHistoryDto dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            return false;
        }

        var history = new UserExchangeHistory()
        {
            UserID = dto.UserID,
            RateID = dto.RateID,
            Amount = dto.Amount,
            AccountID = dto.AccountID,
            PaymentStatus = dto.PaymentStatus,
            PaymentType = dto.PaymentType,
        };

        _dbContext.UserExchangeHistories.Add(history);

        if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(AddUserExchangeHistoryAsync)}");
    }

    public async Task<PageResult<UserExchangeHistoryDto>> GetAllUserExchangeHistoryServiceiesAsync(FilterUserExchangeHistoryDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext
         .UserExchangeHistories
         .AsQueryable();

        if (!baseQuery.Any())
        {
            throw new NotFoundException("User exchange history not found");
        }

        if (!string.IsNullOrEmpty(filter.SearchPhrase))
        {
            baseQuery = baseQuery.Where(x => x.UserID.Contains(filter.SearchPhrase));
        }

        var totalItemCount = baseQuery.Count();

        var itemsDto = await baseQuery
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ProjectTo<UserExchangeHistoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<UserExchangeHistoryDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<UserExchangeHistory?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
           .UserExchangeHistories
           .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result;
    }
}