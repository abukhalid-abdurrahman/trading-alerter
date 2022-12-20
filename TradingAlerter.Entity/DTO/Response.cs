using TradingAlerter.CrossCuttingConcerns.Extension;

namespace TradingAlerter.Entity.DTO;

public sealed class Response<T>
{
    public ErrorCode Code { get; set; }
    public string Message { get; set; }

    #nullable enable
    public T? Payload { get; set; }
    #nullable disable
    
    public string[] ErrorMessages { get; set; }

    public static Response<T> FailedResponse(ErrorCode code, string message = null, string[] errorMessages = null) =>
        new Response<T>() 
        {
            Code = code,
            Message = message ?? code.GetDescription(),
            Payload = default,
            ErrorMessages = errorMessages
        };
    
    public static Response<T> SuccessResponse(ErrorCode code, T payload, string message = null) =>
        new Response<T>() 
        {
            Code = code,
            Message = message ?? code.GetDescription(),
            Payload = payload
        };
}
