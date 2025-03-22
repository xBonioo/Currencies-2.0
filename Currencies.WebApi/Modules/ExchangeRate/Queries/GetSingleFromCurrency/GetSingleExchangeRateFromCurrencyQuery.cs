using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingleFromCurrency;

public record GetSingleExchangeRateFromCurrencyQuery(int fromId, int toId) : IRequest<(ExchangeRateDto?, ExchangeRateDto?)>;