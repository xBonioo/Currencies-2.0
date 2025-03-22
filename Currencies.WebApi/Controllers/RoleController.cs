using Currencies.Abstractions.Contracts;
using Currencies.Abstractions.Forms;
using Currencies.Abstractions.Response;
using Currencies.Application.ModelDtos.Role;
using Currencies.WebApi.Modules.Role.Commands.Create;
using Currencies.WebApi.Modules.Role.Commands.Delete;
using Currencies.WebApi.Modules.Role.Commands.Update;
using Currencies.WebApi.Modules.Role.Queries.GetAll;
using Currencies.WebApi.Modules.Role.Queries.GetEditForm;
using Currencies.WebApi.Modules.Role.Queries.GetSingle;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Currencies.WebApi.Controllers;

/// <summary>
/// For information on how to use the various controllers, go to:
/// 'https wiki-link'
/// </summary>
[Authorize]
[Route("api/role")]
[ApiController]
public class RoleController : Controller
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retrieves all available roles.
    /// </summary>
    /// <response code="200">Returns all available currencies.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    public async Task<ActionResult<BaseResponse<PageResult<RoleDto>>>> GetAllRoles([FromQuery] FilterRoleDto filter)
    {
        var result = await _mediator.Send(new GetRolesListQuery(filter));   
        if (result == null)
        {
            return NotFound(new BaseResponse<PageResult<RoleDto>>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no roles"
            });
        }

        return Ok(new BaseResponse<PageResult<RoleDto>>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Returns role by id.
    /// </summary>
    /// <response code="200">Searched role.</response>
    /// <response code="404">Role not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponse<RoleDto>>> GetRoleById(int id)
    {
        var result = await _mediator.Send(new GetSingleRoleQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<RoleDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no role with Id: {id}"
            });
        }

        return Ok(new BaseResponse<RoleDto>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Creates role.
    /// </summary>
    /// <response code="201">Role correctly created.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    [HttpPost]
    public async Task<ActionResult<BaseResponse<RoleDto>>> CreateRole([FromBody] BaseRoleDto dto)
    {
        var result = await _mediator.Send(new CreateRoleCommand(dto));
        if (result == null)
        {
            return BadRequest(new BaseResponse<RoleDto>
            {
                ResponseCode = StatusCodes.Status400BadRequest,
            });
        }

        return CreatedAtAction(nameof(CreateRole), new BaseResponse<RoleDto>
        {
            Message = "Role was created successfully",
            ResponseCode = StatusCodes.Status201Created,
            Data = result
        });
    }

    /// <summary>
    /// Get create/edit form of role.
    /// </summary>
    /// <response code="200">Role edit form correctly response.</response>
    [HttpGet("{id}/edit-form")]
    public async Task<ActionResult<BaseResponse<RoleEditForm>>> GetRoleEditForm(int id)
    {
        var result = await _mediator.Send(new GetRoleEditFormQuery(id));
        if (result == null)
        {
            return NotFound(new BaseResponse<RoleEditForm>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no role with Id: {id}"
            });
        }

        return Ok(new BaseResponse<RoleEditForm>
        {
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Updates role - it's all properties.
    /// </summary>
    /// <response code="200">Role correctly updated.</response>
    /// <response code="400">Please insert correct JSON object with parameters.</response>
    /// <response code="404">Role not found.</response>
    [HttpPost("{id}/edit")]
    public async Task<ActionResult<BaseResponse<RoleDto>>> UpdateRole(int id, [FromBody] BaseRoleDto dto)
    {
        var result = await _mediator.Send(new UpdateRoleCommand(id, dto));
        if (result == null)
        {
            return NotFound(new BaseResponse<RoleDto>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no role with Id: {id}"
            });
        }

        return Ok(new BaseResponse<RoleDto>
        {
            Message = "Role was updated successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }

    /// <summary>
    /// Deletes role. (Changes flag "IsActive" to false - Soft delete))
    /// </summary>
    /// <response code="200">Role correctly deleted.</response>
    /// <response code="404">Role not found.</response>
    [HttpDelete("{id}/delete")]
    public async Task<ActionResult<BaseResponse<bool>>> DeleteRole(int id)
    {
        var result = await _mediator.Send(new DeleteRoleCommand(id));
        if (!result)
        {
            return NotFound(new BaseResponse<bool>
            {
                ResponseCode = StatusCodes.Status404NotFound,
                Message = $"There's no role with Id: {id}"
            });
        }

        return Ok(new BaseResponse<bool>
        {
            Message = "Role was deleted successfully",
            ResponseCode = StatusCodes.Status200OK,
            Data = result
        });
    }
}