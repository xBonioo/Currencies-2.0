using Currencies.Abstractions.Infrastructure;

namespace Currencies.Db.Entities;

public class Role : ICreatable, IModifable
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}
