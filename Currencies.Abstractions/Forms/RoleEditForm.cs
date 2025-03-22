using Currencies.Abstractions.Controls;

namespace Currencies.Abstractions.Forms;

public class RoleEditForm
{
    public StringControl Name { get; set; }
    public BoolControl IsActive { get; set; }
}
