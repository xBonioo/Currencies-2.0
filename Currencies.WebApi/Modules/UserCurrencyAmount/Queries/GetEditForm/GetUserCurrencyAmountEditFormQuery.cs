using Currencies.Abstractions.Forms;
using MediatR;

namespace Currencies.WebApi.Modules.UserCurrencyAmount.Queries.GetEditForm;

public record GetUserCurrencyAmountEditFormQuery(int Id) : IRequest<UserCurrencyAmountEditForm>;
