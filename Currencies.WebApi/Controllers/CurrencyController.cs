using Currencies.Abstractions.Contracts;
using Currencies.Abstractions.Forms;
using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Currency;
using Currencies.WebApi.Modules.Currency.Commands.Create;
using Currencies.WebApi.Modules.Currency.Commands.Delete;
using Currencies.WebApi.Modules.Currency.Commands.Update;
using Currencies.WebApi.Modules.Currency.Queries.GetAll;
using Currencies.WebApi.Modules.Currency.Queries.GetEditForm;
using Currencies.WebApi.Modules.Currency.Queries.GetSingle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currencies.WebApi.Controllers;

/// <summary>
/// For information on how to use the various controllers, go to:
/// 'https wiki-link'
/// </summary>
[Authorize]
[Route("api/currency")]
[ApiController]
public class CurrencyController(IMediator mediator) : Controller
{
    /// <summary>
    /// Retrieves all available currencies.
    /// </summary>
    /// <response code="200">Returns all available currencies.</response>
    /// <response code="500">Internal server error.</response>
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PageResult<CurrencyDto>>>> GetAllCurrencies([FromQuery] FilterCurrencyDto filter)
    {
        var result = await mediator.Send(new GetCurrenciesListQuery(filter));
        if (result == null)
        {
            return NotFound(new BaseResponse<PageResult<CurrencyDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no currencies"
            });
        }

        return Ok(new BaseResponse<PageResult<CurrencyDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Returns currency by id.
    /// </summary>
    /// <response code="200">Searched currency.</response>
    /// <response code="404">Currency not found.</response>
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<CurrencyDto>>> GetCurrencyById(int id)
    {
        var result = await mediator.Send(new GetSingleCurrencyQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<CurrencyDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no currency with Id: {id}"
            });
        }

        return Ok(new BaseResponse<CurrencyDto>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Creates currency.
    /// </summary>
    /// <response code="201">Currency correctly created.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<CurrencyDto>>> CreateCurrency([FromBody] BaseCurrencyDto dto)
    {
        var result = await mediator.Send(new CreateCurrencyCommand(dto));
        if (result == null)
        {
            return BadRequest(new BaseResponse<CurrencyDto>
            {
                ResponseCode = StatusCodes.Status400BadRequest,
            });
        }

        return CreatedAtAction(nameof(CreateCurrency), new BaseResponse<CurrencyDto>
        {
            Message = "Currency was created successfully",
            ResponseCode = StatusCodes.Status201Created,
            Data = result
        });
    }

    /// <summary>
    /// Get create/edit form of currency.
    /// </summary>
    /// <response code="200">Currency edit form correctly response.</response>
    [HttpGet("{id}/edit-form")]
    public async Task<ActionResult<BaseResponse<CurrencyEditForm>>> GetCurrencyEditForm(int id)
    {
        var result = await mediator.Send(new GetCurrencyEditFormQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<CurrencyEditForm>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no currency with Id: {id}"
            });
        }

        return Ok(new BaseResponse<CurrencyEditForm>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Updates currency - it's all properties.
    /// </summary>
    /// <response code="200">Currency correctly updated.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    /// <response code="404">Currency not found.</response>
    [HttpPost("{id}/edit")]
    public async Task<ActionResult<BaseResponse<CurrencyDto>>> UpdateCurrency(int id, [FromBody] BaseCurrencyDto dto)
    {
        var result = await mediator.Send(new UpdateCurrencyCommand(id, dto));
        if (result == null)
        {
            return NotFound(new BaseResponse<CurrencyDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no currency with Id: {id}"
            });
        }

        return Ok(new BaseResponse<CurrencyDto>
        {
            Message = "Currency was updated successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Deletes currency. (Changes flag "IsActive" to false - Soft delete))
    /// </summary>
    /// <response code="200">Currency correctly deleted.</response>
    /// <response code="404">Currency not found.</response>
    [HttpDelete("{id}/delete")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteCurrency(int id)
    {
        var result = await mediator.Send(new DeleteCurrencyCommand(id));
        if (!result)
        {
            return NotFound(new BaseResponse<bool>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no currency with Id: {id}"
            });
        }

        return Ok(new BaseResponse<bool>
        {
            Message = "Currency was deleted successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }
}