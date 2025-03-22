using Currencies.Application.ModelDtos.User.ExchangeHistory;
using MediatR;

namespace Currencies.WebApi.Modules.UserExchangeHistory.Queries.GetSingle;

public record GetSingleUserExchangeHistoryQuery(int Id) : IRequest<UserExchangeHistoryDto>;
