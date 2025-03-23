using AutoMapper;
using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Forms;
using Currencies.Abstractions.Response;
using Currencies.Application;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.Application.Services;
using Xunit;
using Currencies.Db;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Create;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Delete;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Update;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetAll;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetEditForm;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingle;

namespace Currencies.Tests;

public class ExchangeRateControllerTests : IClassFixture<BaseTestFixture>
{
    private readonly TableContext _dbContext;
    private readonly IExchangeRateService _exchangeRateService;
    private readonly IMapper _mapper;
    private TableContext dbContext;

    public ExchangeRateControllerTests(BaseTestFixture fixture)
    {
        _dbContext = fixture._dbContext;
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new AutoMapperProfile());
        });
        _mapper = mappingConfig.CreateMapper();
        _exchangeRateService = new ExchangeRateService(_dbContext, _mapper);
    }

    [Fact]
    public async Task GetAll_ExchangeRate_ReturnPageResult()
    {
        // arrange
        FilterExchangeRateDto filter = new()
        {
            PageNumber = 1,
            PageSize = 10
        };

        GetExchangeRatesListQuery query = new(filter);
        GetExchangeRatesListQueryHandler handler = new(_exchangeRateService);

        // act
        var result = await handler.Handle(query, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<PageResult<ExchangeRateDto>>(result);
    }

    [Fact]
    public async Task GetById_ExchangeRate_ReturnExchangeRate()
    {
        // arrange
        var id = 1;
        var direction = 0;

        GetSingleExchangeRateQuery query = new(id, direction);
        GetSinglExchangeRateQueryHandler handler = new(_exchangeRateService, _mapper);

        // act
        var result = await handler.Handle(query, new CancellationToken());

        // assert
        Assert.NotNull(result);
        var exchangeRateDto = result.FirstOrDefault(u => u.Id == id);
        Assert.NotNull(exchangeRateDto);
        Assert.Equal(id, exchangeRateDto.Id);
    }

    [Fact]
    public async Task Create_ExchangeRate_ReturnNewExchangeRate()
    {
        // arrange
        BaseExchangeRateDto dto = new()
        {
            Rate = 5.02m,
            FromCurrencyId = 4,
            ToCurrencyId = 2,
            Direction = Direction.Sell,
            IsActive = true
        };

        CreateExchangeRateCommand command = new(new DateTime(2024, 5, 31));
        CreateExchangeRateCommandHandler handler = new(_exchangeRateService, _dbContext);

        // act
        var result = await handler.Handle(command, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<List<ExchangeRateDto>>(result);
    }

    [Fact]
    public async Task Update_ExchangeRate_ReturnUpdatedExchangeRate()
    {
        // arrange
        var id = 1;
        BaseExchangeRateDto dto = new()
        {
            Rate = 5.02m,
            FromCurrencyId = 4,
            ToCurrencyId = 2,
            Direction = Direction.Sell,
            IsActive = true
        };

        UpdateExchangeRateCommand command = new(id, dto);
        UpdateExchangeRateCommandHandler handler = new(_exchangeRateService);

        // act
        var result = await handler.Handle(command, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ExchangeRateDto>(result);
    }

    [Fact]
    public async Task Delete_ExchangeRate_ReturnTrue()
    {
        // arrange
        var id = 1;

        DeleteExchangeRateCommand command = new(id);
        DeleteExchangeRateCommandHandler handler = new(_exchangeRateService);

        // act
        var result = await handler.Handle(command, new CancellationToken());

        // assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetEditForm_ExchangeRate_ReturnExchangeRateEmptyEditForm()
    {
        // arrange
        var id = 0;

        GetExchangeRateEditFormQuery query = new(id);
        GetExchangeRateEditFormQueryHandler handler = new(_exchangeRateService);

        // act
        var result = await handler.Handle(query, new CancellationToken());

        // assert
        Assert.NotNull(result);
        Assert.Equal(0, result.FromCurrencyId.Value);
        Assert.Equal(0, result.ToCurrencyId.Value);
        Assert.Equal(0, result.Rate.Value);
        Assert.Null(result.Direction.Value);
        Assert.True(result.IsActive.Value);
        Assert.IsAssignableFrom<ExchangeRateEditForm>(result);
    }
}
