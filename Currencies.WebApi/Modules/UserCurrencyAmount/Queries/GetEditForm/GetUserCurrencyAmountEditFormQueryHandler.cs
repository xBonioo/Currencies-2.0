using AutoMapper;
using Currencies.Abstractions.Controls;
using Currencies.Abstractions.Forms;
using Currencies.Application.Interfaces;
using Currencies.Db;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetEditForm;

public class GetUserCurrencyAmountEditFormQueryHandler : IRequestHandler<GetUserCurrencyAmountEditFormQuery, UserCurrencyAmountEditForm?>
{
    private readonly IUserCurrencyAmountService _userCurrencyAmountService;
    private readonly IMapper _mapper;
    private readonly TableContext _dbContext;

    public GetUserCurrencyAmountEditFormQueryHandler(IUserCurrencyAmountService userCurrencyAmountService, IMapper mapper, TableContext dbContext)
    {
        _userCurrencyAmountService = userCurrencyAmountService;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<UserCurrencyAmountEditForm> Handle(GetUserCurrencyAmountEditFormQuery request, CancellationToken cancellationToken)
    {
        var userCurrencyAmount = await _userCurrencyAmountService.GetByIdAsync(request.Id, cancellationToken);
        if (userCurrencyAmount == null || !userCurrencyAmount.IsActive)
        {
            var createForm = new UserCurrencyAmountEditForm()
            {
                UserId = new StringControl()
                {
                    IsRequired = true,
                    Value = string.Empty,
                    MinLenght = 1,
                    MaxLenght = 15
                },
                CurrencyId = new IntegerControl()
                {
                    IsRequired = true,
                    Value = 0,
                    MinValue = 1,
                    MaxValue = 999999999
                },
                Amount = new DecimalControl()
                {
                    IsRequired = true,
                    Value = 0.0m,
                    MinValue = 0.1m,
                    MaxValue = 9999999999
                },
                IsActive = new BoolControl()
                {
                    IsRequired = true,
                    Value = true
                }
            };


            return createForm;
        }

        var editForm = new UserCurrencyAmountEditForm()
        {
             UserId = new StringControl()
             {
                 IsRequired = true,
                 Value = userCurrencyAmount.UserId,
                 MinLenght = 1,
                 MaxLenght = 15
             },
            CurrencyId = new IntegerControl()
            {
                IsRequired = true,
                Value = userCurrencyAmount.CurrencyId,
                MinValue = 1,
                MaxValue = 999999999
            },
            Amount = new DecimalControl()
            {
                IsRequired = true,
                Value = userCurrencyAmount.Amount,
                MinValue = 0.1m,
                MaxValue = 9999999999
            },
            IsActive = new BoolControl()
            {
                IsRequired = true,
                Value = true
            }
        };

        return editForm;
    }
}
