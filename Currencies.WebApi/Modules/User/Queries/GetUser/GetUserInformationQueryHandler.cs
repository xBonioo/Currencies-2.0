using AutoMapper;
using Currencies.Abstractions.Contracts.Exceptions;
using Currencies.Application.ModelDtos.User;
using Currencies.Db;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Currencies.WebApi.Modules.User.Queries.GetUser;

public class GetUserInformationQueryHandler(TableContext dbContext, IMapper mapper)
    : IRequestHandler<GetUserInformationQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserInformationQuery request, CancellationToken cancellationToken)
    {
        var result = await dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == request.request.UserId, cancellationToken);

        if (result == null)
        {
            throw new NotFoundException("User not found");
        }

        return mapper.Map<UserDto>(result);
    }
}