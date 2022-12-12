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
    [Description("Smtp not available or e-mail message not valid.")]
    SmtpError = 23,
    [Description("E-mail receipient error.")]
    SmtpReceipientError = 24,
    [Description("Upstream service not available. Please retry later...")]
    UpstreamServiceUnavailable = 25
}
