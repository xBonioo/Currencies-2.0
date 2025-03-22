using Currencies.Abstractions.Forms;
using MediatR;

namespace Currencies.WebApi.Modules.ExchangeRate.Queries.GetEditForm;

public record GetExchangeRateEditFormQuery(int id) : IRequest<ExchangeRateEditForm?>;