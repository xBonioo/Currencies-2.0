using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Controls;

namespace Currencies.Abstractions.Forms;

public class ExchangeRateEditForm
{
    public IntegerControl FromCurrencyId { get; set; }
    public IntegerControl ToCurrencyId { get; set; }
    public DecimalControl Rate { get; set; }
    public EnumControl<Direction> Direction { get; set; }
    public BoolControl IsActive { get; set; }
}
