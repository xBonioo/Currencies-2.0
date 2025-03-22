using System.Text.Json;
using Currencies.Abstractions.Contracts;
using Currencies.Abstractions.Contracts.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Currencies.Application.Middleware;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException validationException)
        {
            var response = new BaseResponse<IEnumerable<ValidationFailure>>
            {
                ResponseCode = StatusCodes.Status422UnprocessableEntity,
                Message = $"One or more validation errors has occurred.",
                BaseResponseError = validationException.Errors
                    .Select(x => new BaseResponseError(x.PropertyName, x.ErrorCode, x.ErrorMessage)).ToList()
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (BadRequestException badRequestException)
        {
            var response = new BaseResponse<IEnumerable<string>>
            {
                ResponseCode = StatusCodes.Status400BadRequest,
                Message = $"Some server error has occurred. {badRequestException.Message}"
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (NotFoundException notFoundException)
        {
            var response = new BaseResponse<IEnumerable<string>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"The item you were looking for was not found. {notFoundException.Message}"
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (DbUpdateException dbException)
        {
            var response = new BaseResponse<IEnumerable<string>>
            {
                ResponseCode = StatusCodes.Status500InternalServerError,
                Message = $"{dbException.Message}."
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception)
        {
            var response = new BaseResponse<string>
            {
                ResponseCode = StatusCodes.Status500InternalServerError,
                Message = $"Some server error has occured."
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}