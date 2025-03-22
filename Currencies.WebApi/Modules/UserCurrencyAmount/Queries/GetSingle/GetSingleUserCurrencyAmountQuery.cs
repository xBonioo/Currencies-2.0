using Currencies.Application.ModelDtos.User.CurrencyAmount;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetSingle;

public record GetSingleUserCurrencyAmountQuery(string Id) : IRequest<List<UserCurrencyAmountDto>>;
