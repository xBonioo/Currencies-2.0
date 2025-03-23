using AutoMapper;
using Currencies.Abstractions.Forms;
using Currencies.Abstractions.Response;
using Currencies.Application;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using Currencies.Application.Services;
using Currencies.Db;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Create;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Delete;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;
using Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetAll;
using Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetEditForm;
using Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetSingle;
using Xunit;

namespace Currencies.Tests;


public class UserCurrencyAmountControllerTests : IClassFixture<BaseTestFixture>
{
    private readonly TableContext _dbContext;
    private readonly IUserCurrencyAmountService _userCurrencyAmountService;
    private readonly IMapper _mapper;

    public UserCurrencyAmountControllerTests(BaseTestFixture fixture)
    {
        _dbContext = fixture._dbContext;
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });
        _mapper = mappingConfig.CreateMapper();
        _userCurrencyAmountService = new UserCurrencyAmountService(_dbContext, _mapper);
    }

    [Fact]
    public async Task GetAll_UserCurrencyAmounts_ReturnPageResult()
    {
        // arrange
        FilterUserCurrencyAmountDto filter = new()
        {
            PageNumber = 1,
            PageSize = 10
        };

        GetUserCurrencyAmountsListQuery query = new(filter);
        GetUserCurrencyAmountsListQueryHandler handler = new(_userCurrencyAmountService);

        // act
        var result = await handler.Handle(query, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<PageResult<UserCurrencyAmountDto>>(result);
    }

    [Fact]
    public async Task GetById_UserCurrencyAmounts_ReturnUserCurrencyAmount()
    {
        // arrange
        var id = "679381f2-06a1-4e22-beda-179e8e9e3236";

        GetSingleUserCurrencyAmountQuery query = new(id);
        GetSinglUserCurrencyAmountQueryHandler handler = new(_userCurrencyAmountService, _mapper);

        // act
        var result = await handler.Handle(query, new CancellationToken());

        // assert
        Assert.NotNull(result);
        var userCurrencyAmountDto = result.FirstOrDefault(u => u.UserId == id);
        Assert.NotNull(userCurrencyAmountDto);
        Assert.Equal(id, userCurrencyAmountDto.UserId);
    }

    [Fact]
    public async Task Create_UserCurrencyAmount_ReturnNewUserCurrencyAmount()
    {
        // arrange
        BaseUserCurrencyAmountDto dto = new()
        {
            UserId = "679381f2-06a1-4e22-beda-179e8e9e3236",
            CurrencyId = 4,
            Amount = 100,
            IsActive = true
        };

        CreateUserCurrencyAmountCommand command = new(dto);
        CreateUserCurrencyAmountCommandHandler handler = new(_userCurrencyAmountService);

        // act
        var result = await handler.Handle(command, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<UserCurrencyAmountDto>(result);
    }

    [Fact]
    public async Task Update_UserCurrencyAmount_ReturnUpdatedUserCurrencyAmount()
    {
        // arrange
        var id = 1;
        BaseUserCurrencyAmountDto dto = new()
        {
            UserId = "679381f2-06a1-4e22-beda-179e8e9e3236",
            CurrencyId = 4,
            Amount = 200,
            IsActive = true
        };

        UpdateUserCurrencyAmountCommand command = new(id, dto);
        UpdateUserCurrencyAmountCommandHandler handler = new(_userCurrencyAmountService);

        // act
        var result = await handler.Handle(command, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<UserCurrencyAmountDto>(result);
    }

    [Fact]
    public async Task Delete_UserCurrencyAmount_ReturnTrue()
    {
        // arrange
        var id = 1;

        DeleteUserCurrencyAmountCommand command = new(id);
        DeleteUserCurrencyAmountCommandHandler handler = new(_userCurrencyAmountService);

        // act
        var result = await handler.Handle(command, new CancellationToken());

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetEditForm_UserCurrencyAmount_ReturnUserCurrencyAmountEmptyEditForm()
    {
        // arrange
        var id = 0;

        GetUserCurrencyAmountEditFormQuery query = new(id);
        GetUserCurrencyAmountEditFormQueryHandler handler = new(_userCurrencyAmountService, _mapper, _dbContext);

        // act
        var result = await handler.Handle(query, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.Equal(string.Empty, result.UserId.Value);
        Assert.Equal(0, result.CurrencyId.Value);
        Assert.Equal(0, result.Amount.Value);
        Assert.True(result.IsActive.Value);
        Assert.IsAssignableFrom<UserCurrencyAmountEditForm>(result);
    }
}
