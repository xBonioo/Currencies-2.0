using Currencies.Abstractions.Contracts.Enum;
using Currencies.Abstractions.Infrastructure;

namespace Currencies.Db.Entities;

public class ExchangeRate : ICreatable, IModifable
{
    public int Id { get; set; }
    public int FromCurrencyID { get; set; }
    public int ToCurrencyID { get; set; }
    public decimal Rate { get; set; }
    public Direction Direction { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }

    public virtual Currency FromCurrency { get; set; } = null!;
    public virtual Currency ToCurrency { get; set; } = null!;
}