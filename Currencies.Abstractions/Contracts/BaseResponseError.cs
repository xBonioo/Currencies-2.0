namespace Currencies.Abstractions.Contracts;

public class BaseResponseError
{
    public string? PropertyName { get; set; }
    public string Message { get; set; }
    public string? Code { get; set; }

    public BaseResponseError(string message)
    {
        Message = message;
    }

    public BaseResponseError(string? propertyName, string message, string? code)
    {
        PropertyName = propertyName;
        Message = message;
        Code = code;
    }
}