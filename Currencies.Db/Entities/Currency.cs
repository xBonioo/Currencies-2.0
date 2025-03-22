using Currencies.Abstractions.Infrastructure;

namespace Currencies.Db.Entities;

public class Currency : ICreatable, IModifable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}