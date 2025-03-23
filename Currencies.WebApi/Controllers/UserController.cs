using Currencies.Abstractions.Contracts;
using Currencies.Abstractions.Requests;
using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.User;
using Currencies.Application.ModelDtos.User.ExchangeHistory;
using Currencies.WebApi.Modules.User.Commands.RefreshToken;
using Currencies.WebApi.Modules.User.Commands.Register;
using Currencies.WebApi.Modules.User.Commands.SignIn;
using Currencies.WebApi.Modules.User.Commands.SignOut;
using Currencies.WebApi.Modules.User.Queries.GetUser;
using Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetAll;
using Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetSingle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currencies.WebApi.Controllers;

/// <summary>
/// For information on how to use the various controllers, go to:
/// 'https wiki-link'
/// </summary>
//[Authorize]
[Route("api/user")]
[ApiController]
public class UserController(IMediator mediator) : Controller
{
    /// <summary>
    /// Register new User and send confirmation email to User's email.
    /// </summary>
    /// <param name="registerUser">JSON object with properties defining a user to create</param>
    /// <response code="201">User was created successfully and confirmation email was sent.</response>
    /// <response code="500">Confirmation link could not be created.</response>
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<BaseResponse<PageResult<UserDto>>>> RegisterUserAsync([FromBody] RegisterUserDto registerUser)
    {
        var result = await mediator
            .Send(new RegisterUserCommand
            {
                RegisterUserDto = registerUser
            });

        return CreatedAtAction(nameof(RegisterUserAsync), new BaseResponse<UserDto>
        {
            ResponseCode = StatusCodes.Status201Created,
            Data = result,
            Message = "User was created successfully and confirmation email was sent."
        });
    }

    /// <summary>
    /// Action to sign in a user by username and password
    /// </summary>
    /// <param name="dto">JSON object with username and password</param>
    /// <response code="200">Successfully signed in</response>
    /// <response code="400">Username or password is not valid</response>
    /// <response code="500">Internal server error</response>
    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<ActionResult<BaseResponse<RefreshTokenResponse>>> SignInUserAsync([FromBody] SignInDto dto)
    {
        var response = await mediator.Send(new SignInCommand(dto));

        if (response is null)
        {
            return BadRequest(new BaseResponse<object>
            {
                ResponseCode = StatusCodes.Status400BadRequest,
                Data = null,
                Message = "Username or password is not valid"
            });
        }

        return Ok(new BaseResponse<RefreshTokenResponse>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = response,
            Message = "You have been signed in successfully"
        });
    }

    /// <summary>
    /// Action to sign out the user. The user's refresh token is beeing deleted from database.
    /// </summary>
    /// <response code="200">Successfully signed out</response>
    /// <response code="401">Something wrong with given tokens propably</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("signout")]
    public async Task<ActionResult<BaseResponse<bool>>> SignOutUserAsync()
    {
        HttpContext.Request.Headers.TryGetValue("Authorization", out var accessToken);
        await mediator.Send(new SignOutCommand(accessToken.ToString().Split(' ').Last()));

        return Ok(new BaseResponse<bool>
        {
            ResponseCode = StatusCodes.Status200OK,
            Message = "You have been signed out successfully"
        });
    }

    /// <summary>
    /// Generates a new acccess token (JWT). It is necessary to give an old access token in header 
    /// (the same way as when authorizing to protected resorces)
    /// and active refresh token in body
    /// </summary>
    /// <param name="refreshToken">A refresh token given afler signing in (and refreshing access token)</param>
    /// <response code="200">New access token and refresh token</response>
    /// <response code="401">Something wrong with given tokens propably</response>
    /// <response code="500">Internal server error</response>
    [HttpPost("refreshtoken")]
    public async Task<ActionResult<BaseResponse<RefreshTokenResponse>>> RefreshTokenAsync([FromBody] RefreshTokenDto refreshToken)
    {
        var isAccessTokenGiven = HttpContext.Request.Headers.TryGetValue("Authorization", out var accessToken);
        
        if (!isAccessTokenGiven)
        {
            return Unauthorized(new BaseResponse<object>
            {
                ResponseCode = StatusCodes.Status401Unauthorized,
                Data = null,
                Message = "Failed to generate new token"
            });
        }

        var response = await mediator.Send(new RefreshTokenCommand(refreshToken.RefreshToken, 
                                                                    accessToken.ToString().Split(' ')[1]));

        if (response is null)
        {
            return Unauthorized(new BaseResponse<object>
            {
                ResponseCode = StatusCodes.Status401Unauthorized,
                Data = null,
                Message = "Failed to generate new token"
            });

        }

        return Ok(new BaseResponse<RefreshTokenResponse>
        {
            ResponseCode = StatusCodes.Status401Unauthorized,
            Data = response,
            Message = "Here is your new access token and refresh token"
        });
    }

    /// <summary>
    /// Retrieves all available user exchange histories
    /// </summary>
    /// <response code="200">Returns all available user exchange histories.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet("get-history")]
    public async Task<ActionResult<BaseResponse<PageResult<UserExchangeHistoryDto>>>> GetAllUserExchangeHistories([FromQuery] FilterUserExchangeHistoryDto filter)
    {
        var result = await mediator.Send(new GetUserExchangeHistoryListQuery(filter));
        if (result is null)
        {
            return NotFound(new BaseResponse<PageResult<UserExchangeHistoryDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user exchange histories"
            });
        }

        return Ok(new BaseResponse<PageResult<UserExchangeHistoryDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Returns user exchange histories by id.
    /// </summary>
    /// <response code="200">Searched user exchange histories.</response>
    /// <response code="404">User exchange histories not found.</response>
    [HttpGet("get-history/{id}")]
    public async Task<ActionResult<BaseResponse<UserExchangeHistoryDto>>> GetUserExchangeHistoryById(int id)
    {
        var result = await mediator.Send(new GetSingleUserExchangeHistoryQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<UserExchangeHistoryDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user exchange histories with Id: {id}"
            });
        }

        return Ok(new BaseResponse<UserExchangeHistoryDto>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Retrieves user information
    /// </summary>
    /// <response code="200">Returns all available user exchange histories.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost("get-user")]
    public async Task<ActionResult<BaseResponse<UserDto>>> GetUserInformation([FromBody] GetUserRequest filter)
    {
        var result = await mediator.Send(new GetUserInformationQuery(filter));
        if (result is null)
        {
            return NotFound(new BaseResponse<UserDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no user exchange histories"
            });
        }

        return Ok(new BaseResponse<UserDto>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }
}