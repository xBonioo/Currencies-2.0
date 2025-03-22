namespace Currencies.Abstractions.Controls;

public class EnumControl<T> where T : struct, Enum
{
    public bool IsRequired { get; set; }
    public T? Value { get; set; }

    public bool IsValid()
    {
        if (!IsRequired && Value == null)
        {
            return true;
        }

        return Value.HasValue && Enum.IsDefined(typeof(T), Value.Value);
    }
}
