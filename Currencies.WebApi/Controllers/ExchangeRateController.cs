using Currencies.Abstractions.Contracts;
using Currencies.Abstractions.Forms;
using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.ExchangeRate;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Create;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Delete;
using Currencies.WebApi.Modules.ExchangeRate.Commands.Update;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetAll;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetEditForm;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingle;
using Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingleFromCurrency;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Currencies.WebApi.Controllers;

/// <summary>
/// For information on how to use the various controllers, go to:
/// 'https wiki-link'
/// </summary>
[Authorize]
[Route("api/exchange")]
[ApiController]
public class ExchangeRateController : Controller
{
    private readonly IMediator _mediator;

    public ExchangeRateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all available exchange rates.
    /// </summary>
    /// <response code="200">Returns all available exchange rates.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PageResult<ExchangeRateDto>>>> GetAllExchangeRates([FromQuery] FilterExchangeRateDto filter)
    {
        var result = await _mediator.Send(new GetExchangeRatesListQuery(filter));
        if (result is null)
        {
            return NotFound(new BaseResponse<PageResult<ExchangeRateDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no currencies"
            });
        }

        return Ok(new BaseResponse<PageResult<ExchangeRateDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Returns exchange rate list by id.
    /// </summary>
    /// <response code="200">Searched exchange rate.</response>
    /// <response code="404">Exchange rate not found.</response>
    [AllowAnonymous]
    [HttpGet("{direction}/{id}")]
    public async Task<ActionResult<BaseResponse<List<ExchangeRateDto>>>> GetExchangeRateById(int id, int direction)
    {
        var result = await _mediator.Send(new GetSingleExchangeRateQuery(id, direction));
        if (result == null)
        {
            return NotFound(new BaseResponse<List<ExchangeRateDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no exchange rate"
            });
        }

        return Ok(new BaseResponse<List<ExchangeRateDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Returns exchange rate by currencies ids.
    /// </summary>
    /// <response code="200">Searched exchange rate.</response>
    /// <response code="404">Exchange rate not found.</response>
    [AllowAnonymous]
    [HttpGet("from/{fromId}/to/{toId}")]
    public async Task<IActionResult> GetExchangeRateByCurrencyId(int fromId, int toId)
    {
        var result = await _mediator.Send(new GetSingleExchangeRateFromCurrencyQuery(fromId, toId));
        if(result == (null, null))
        {
            return NotFound(new BaseResponse<(ExchangeRateDto, ExchangeRateDto)>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no exchange rate"
            });
        }

        var response = new BaseResponse<(ExchangeRateDto?, ExchangeRateDto?)>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        };

        var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include
        });

        return Ok(json);
    }

    /// <summary>
    /// Retrieves exchange rates from the National Bank of Poland (NBP) API for specified currency and date.
    /// </summary>
    /// <param name="date">The date for which exchange rates are requested in the format YYYY-MM-DD. (Only working days)</param>
    /// <returns>Returns a list of currency exchange rates from the NBP API or an error message if something goes wrong.</returns>
    /// <response code="201">Returns the list of exchange rates for the requested currency and date.</response>
    /// <response code="400">Bad request - the input parameters are invalid (e.g., wrong date format or unsupported currency code).</response>
    /// <response code="404">Not found - no exchange rates found for the provided currency and date.</response>
    /// <response code="500">Internal server error - error during processing the request.</response>
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<BaseResponse<List<ExchangeRateDto>>>> CreateExchangeRate([FromBody] DateTime date)
    {
        if (IsHoliday(date))
        {
            return BadRequest(new BaseResponse<ExchangeRateDto>
            {
                Message = "The specified date is a non-working day.",
                ResponseCode = StatusCodes.Status400BadRequest,
            });
        }

        var result = await _mediator.Send(new CreateExchangeRateCommand(date));
        if (result == null)
        {
            return BadRequest(new BaseResponse<ExchangeRateDto>
            {
                ResponseCode = StatusCodes.Status400BadRequest,
            });
        }

        return CreatedAtAction(nameof(CreateExchangeRate), new BaseResponse<List<ExchangeRateDto>>
        {
            Message = "ExchangeRates were created successfully",
            ResponseCode = StatusCodes.Status201Created,
            Data = result
        });
    }

    /// <summary>
    /// Get edit form of exchange rate.
    /// </summary>
    /// <response code="200">Exchange rate edit form correctly response.</response>
    [HttpGet("{id}/edit-form")]
    public async Task<ActionResult<BaseResponse<ExchangeRateEditForm?>>> GetExchangeRateEditForm(int id)
    {
        var result = await _mediator.Send(new GetExchangeRateEditFormQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<ExchangeRateEditForm?>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no exchange rate with Id: {id}"
            });
        }

        return Ok(new BaseResponse<ExchangeRateEditForm>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Updates exchange rate - it's all properties.
    /// </summary>
    /// <response code="200">Exchange rate correctly updated.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    /// <response code="404">Exchange rate not found.</response>
    [HttpPost("{id}/edit")]
    public async Task<ActionResult<BaseResponse<ExchangeRateDto>>> UpdateExchangeRate(int id, [FromBody] BaseExchangeRateDto dto)
    {
        var result = await _mediator.Send(new UpdateExchangeRateCommand(id, dto));
        if (result == null)
        {
            return NotFound(new BaseResponse<ExchangeRateDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no exchange rate with Id: {id}"
            });
        }

        return Ok(new BaseResponse<ExchangeRateDto>
        {
            Message = "Exchange rate was updated successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Deletes exchange rate. (Changes flag "IsActive" to false - Soft delete))
    /// </summary>
    /// <response code="200">Exchange rate correctly deleted.</response>
    /// <response code="404">Exchange rate not found.</response>
    [HttpDelete("{id}/delete")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteExchangeRate(int id)
    {
        var result = await _mediator.Send(new DeleteExchangeRateCommand(id));
        if (!result)
        {
            return NotFound(new BaseResponse<bool>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no exchange rate with Id: {id}"
            });
        }

        return Ok(new BaseResponse<bool>
        {
            Message = "Exchange rate was deleted successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    private bool IsHoliday(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        var holidays = new List<DateTime>
        {
            new DateTime(date.Year, 1, 1),    // Nowy Rok
            new DateTime(date.Year, 5, 1),    // Święto Pracy
            new DateTime(date.Year, 5, 3),    // Święto Konstytucji 3 Maja
            new DateTime(date.Year, 11, 11),  // Dzień Niepodległości
            new DateTime(date.Year, 12, 25),  // Pierwszy dzień Bożego Narodzenia
            new DateTime(date.Year, 12, 26)   // Drugi dzień Bożego Narodzenia
        };

        holidays.Add(CalculateEasterSunday(date.Year));

        return holidays.Contains(date);
    }

    private DateTime CalculateEasterSunday(int year)
    {
        int day = 0;
        int month = 0;

        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        month = (h + l - 7 * m + 114) / 31;
        day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateTime(year, month, day);
    }

}