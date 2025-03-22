namespace Currencies.Abstractions.Contracts;

public class BaseResponse<T>
{
    public int ResponseCode { get; set; }
    public List<BaseResponseError>? BaseResponseError { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
}