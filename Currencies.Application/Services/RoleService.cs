using AutoMapper;
using AutoMapper.QueryableExtensions;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Abstractions.Response;
using Currencies.Application.Interfaces;
using Currencies.Application.ModelDtos.Role;
using Currencies.Db;
using Currencies.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Currencies.Application.Services;

public class RoleService : IRoleService
{
    private readonly TableContext _dbContext;
    private readonly IMapper _mapper;

    public RoleService(TableContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<RoleDto?> CreateAsync(BaseRoleDto dto, CancellationToken cancellationToken)
    {
        if (dto == null)
        {
            return null;
        }

        var role = new Role()
        {
            Name = dto.Name
        };

        _dbContext.Roles.Add(role);

        if (await _dbContext.SaveChangesAsync(cancellationToken) > 0)
        {
            return _mapper.Map<RoleDto>(role);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(CreateAsync)}");

    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var role = await GetByIdAsync(id, cancellationToken);
        if (role == null || !role.IsActive)
        {
            throw new NotFoundException("Role not found");
        }

        role.IsActive = false;

        if ((await _dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return true;
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(DeleteAsync)}");
    }

    public async Task<PageResult<RoleDto>?> GetAllRolesAsync(FilterRoleDto filter, CancellationToken cancellationToken)
    {
        var baseQuery = _dbContext
           .Roles
           .AsQueryable();

        if (!baseQuery.Any())
        {
            throw new NotFoundException("Roles not found");
        }

        if (!string.IsNullOrEmpty(filter.SearchPhrase))
        {
            baseQuery = baseQuery.Where(x => x.Name.Contains(filter.SearchPhrase));
        }
        if (filter.IsActive != null)
        {
            baseQuery = baseQuery.Where(x => x.IsActive == filter.IsActive);
        }

        var totalItemCount = baseQuery.Count();

        var itemsDto = await baseQuery
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PageResult<RoleDto>(itemsDto, totalItemCount, filter.PageSize, filter.PageNumber);
    }

    public async Task<RoleDto?> UpdateAsync(int id, BaseRoleDto dto, CancellationToken cancellationToken)
    {
        var role = await GetByIdAsync(id, cancellationToken);
        if (role == null || !role.IsActive)
        {
            throw new NotFoundException("Role not found");
        }

        role.Name = dto.Name;
        role.IsActive = dto.IsActive;

        if ((await _dbContext.SaveChangesAsync(cancellationToken)) > 0)
        {
            return _mapper.Map<RoleDto>(role);
        }

        throw new DbUpdateException($"Could not save changes to database at: {nameof(UpdateAsync)}");
    }

    public async Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _dbContext
            .Roles
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return result;
    }
}