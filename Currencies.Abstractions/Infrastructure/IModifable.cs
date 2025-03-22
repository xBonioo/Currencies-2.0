namespace Currencies.Abstractions.Infrastructure;

public interface IModifable
{
    public DateTime? ModifiedOn { get; set; }
}