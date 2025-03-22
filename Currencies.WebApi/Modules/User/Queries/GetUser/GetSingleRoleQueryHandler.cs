using AutoMapper;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Application.ModelDtos.User;
using Currencies.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currencies.WebApi.Modules.User.Queries.GetUser;

public class GetUserInformationQueryHandler : IRequestHandler<GetUserInformationQuery, UserDto?>
{
    private readonly TableContext _dbContext;
    private readonly IMapper _mapper;

    public GetUserInformationQueryHandler(TableContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == request.request.UserId, cancellationToken);

        if (result == null)
        {
            throw new NotFoundException("User not found");
        }

        return _mapper.Map<UserDto>(result);
    }
}