using Currencies.Abstractions.Contracts;
using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Forms;
using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User.CurrencyAmount;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Convert;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Create;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Delete;
using Currencies.WebApi.Modules.UserCurrencyAmount.Commands.Update;
using Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetAll;
using Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetEditForm;
using Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetSingle;
using Currencies.WebApi.Modules.UserExchangeHistory.Commands.Add;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currencies.WebApi.Controllers;

/// <summary>
/// For information on how to use the various controllers, go to:
/// 'https wiki-link'
/// </summary>
[Authorize]
[Route("api/user-amount")]
[ApiController]
public class UserCurrencyAmountController(IMediator mediator) : Controller
{
    /// <summary>
    /// Retrieves all available user currency amounts.
    /// </summary>
    /// <response code="200">Returns all available user currency amounts.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PageResult<UserCurrencyAmountDto>>>> GetAllUserCurrencyAmounts([FromQuery] FilterUserCurrencyAmountDto filter)
    {
        var result = await mediator.Send(new GetUserCurrencyAmountsListQuery(filter));
        if (result == null)
        {
            return NotFound(new BaseResponse<PageResult<UserCurrencyAmountDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user currency amounts"
            });
        }

        return Ok(new BaseResponse<PageResult<UserCurrencyAmountDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Returns user currency amount list by user id.
    /// </summary>
    /// <response code="200">Searched user currency amount.</response>
    /// <response code="404">User currency amount not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<List<UserCurrencyAmountDto>>>> GetUserCurrencyAmountById(string id)
    {
        var result = await mediator.Send(new GetSingleUserCurrencyAmountQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<List<UserCurrencyAmountDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user currency amount with Id: {id}"
            });
        }

        return Ok(new BaseResponse<List<UserCurrencyAmountDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Convert currency.
    /// </summary>
    /// <response code="200">Convert user currency amount.</response>
    /// <response code="404">User currency amount not found or something problem.</response>
    [HttpPost("convert")]
    public async Task<ActionResult<BaseResponse<UserCurrencyAmountDto>>> ConvertUserCurrencyAmount([FromBody] ConvertUserCurrencyAmountDto dto)
    {
        var result = await mediator.Send(new ConvertUserCurrencyAmountCommand(dto));
        if (result == null)
        {
            return NotFound(new BaseResponse<UserCurrencyAmountDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's something wrong with convert."
            });
        }

        var history = await mediator.Send(new AddUserExchangeHistoryCommand(new UserExchangeHistoryDto()
        {
            UserId = dto.UserId,
            RateId = result.RateId,
            Amount = dto.Amount,
            AccountId = result.Id,
            PaymentStatus = PaymentStatus.Completed,
            PaymentType = null
        }));

        if (!history)
        {
            return BadRequest(new BaseResponse<UserCurrencyAmountDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's something wrong with add user exchange history."
            });
        }

        return Ok(new BaseResponse<UserCurrencyAmountDto>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Get create/edit form of user currency amount.
    /// </summary>
    /// <response code="200">User currency amount edit form correctly response.</response>
    [HttpGet("{id}/edit-form")]
    public async Task<ActionResult<BaseResponse<UserCurrencyAmountEditForm>>> GetUserCurrencyAmountEditForm(int id)
    {
        var result = await mediator.Send(new GetUserCurrencyAmountEditFormQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<UserCurrencyAmountEditForm>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user currency amount with Id: {id}"
            });
        }

        return Ok(new BaseResponse<UserCurrencyAmountEditForm>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Adds user currency amount.
    /// </summary>
    /// <response code="201">User currency amount correctly added.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    [HttpPost("add")]
    public async Task<ActionResult<BaseResponse<UserCurrencyAmountDto>>> AddUserCurrencyAmount([FromBody] BaseUserCurrencyAmountDto? dto)
    {
        var result = await mediator.Send(new CreateUserCurrencyAmountCommand(dto));
        if (result == null)
        {
            return BadRequest(new BaseResponse<UserCurrencyAmountDto>
            {
                ResponseCode = StatusCodes.Status400BadRequest,
            });
        }

        // random payment type xd
        var random = new Random();
        Array values = Enum.GetValues(typeof(PaymentType));
        PaymentType randomPaymentType = (PaymentType)values.GetValue(random.Next(values.Length));

        var history = await mediator.Send(new AddUserExchangeHistoryCommand(new UserExchangeHistoryDto()
        {
            UserId = dto.UserId,
            RateId = result.RateId,
            Amount = dto.Amount,
            AccountId = result.Id,
            PaymentStatus = PaymentStatus.Completed,
            PaymentType = randomPaymentType
        }));

        if (!history)
        {
            return BadRequest(new BaseResponse<UserCurrencyAmountDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's something wrong with add user exchange history."
            });
        }

        return CreatedAtAction(nameof(AddUserCurrencyAmount), new BaseResponse<UserCurrencyAmountDto>
        {
            Message = "User currency amount was created successfully",
            ResponseCode = StatusCodes.Status201Created,
            Data = result
        });
    }

    /// <summary>
    /// Updates user currency amount - it's all properties.
    /// </summary>
    /// <response code="200">User currency amount correctly updated.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    /// <response code="404">User currency amount not found.</response>
    [HttpPost("{id}/edit")]
    public async Task<ActionResult<BaseResponse<UserCurrencyAmountDto>>> UpdateUserCurrencyAmount(int id, [FromBody] BaseUserCurrencyAmountDto dto)
    {
        var result = await mediator.Send(new UpdateUserCurrencyAmountCommand(id, dto));
        if (result == null)
        {
            return NotFound(new BaseResponse<UserCurrencyAmountDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user currency amount with Id: {id}"
            });
        }

        return Ok(new BaseResponse<UserCurrencyAmountDto>
        {
            Message = "User currency amount was updated successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Deletes user currency amount. (Changes flag "IsActive" to false - Soft delete))
    /// </summary>
    /// <response code="200">User currency amount correctly deleted.</response>
    /// <response code="404">User currency amount not found.</response>
    [HttpDelete("{id}/delete")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteUserCurrencyAmount(int id)
    {
        var result = await mediator.Send(new DeleteUserCurrencyAmountCommand(id));
        if (!result)
        {
            return NotFound(new BaseResponse<bool>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user currency amount with Id: {id}"
            });
        }

        return Ok(new BaseResponse<bool>
        {
            Message = "User currency amount was deleted successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }
}
