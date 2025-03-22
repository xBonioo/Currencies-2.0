namespace Currencies.Abstractions.Contracts.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {

    }
}
