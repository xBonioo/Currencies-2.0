using Currencies.Abstractions.Controls;

namespace Currencies.Abstractions.Forms;

public class CurrencyEditForm
{
    public StringControl Name { get; set; }
    public StringControl Symbol { get; set; }
    public StringControl Description { get; set; }
    public BoolControl IsActive { get; set; }
}
