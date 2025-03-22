using Currencies.Application.ModelDtos.ExchangeRate;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetSingle;

public record GetSingleExchangeRateQuery(int id, int direction) : IRequest<List<ExchangeRateDto>>;