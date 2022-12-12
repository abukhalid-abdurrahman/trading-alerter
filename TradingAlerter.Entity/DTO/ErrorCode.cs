using System.ComponentModel;

namespace TradingAlerter.Entity.DTO;

public enum ErrorCode
{
    [Description("Request success.")]
    Approved = 10,
    [Description("Request failed, internal error.")]
    InternalError = 20,
    [Description("Bad request.")]
    BadRequest = 21,
    [Description("Data not found.")]
    NotFound = 22,
}
