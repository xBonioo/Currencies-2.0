using Currencies.Application.ModelDtos.Currency;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetSingle;

public record GetSingleCurrencyQuery(int id) : IRequest<CurrencyDto>;