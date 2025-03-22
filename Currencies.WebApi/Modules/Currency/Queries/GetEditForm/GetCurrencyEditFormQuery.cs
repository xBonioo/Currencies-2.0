using Currencies.Abstractions.Forms;
using MediatR;

namespace Currencies.WebApi.Modules.Currency.Queries.GetEditForm;

public record GetCurrencyEditFormQuery(int id) : IRequest<CurrencyEditForm>;