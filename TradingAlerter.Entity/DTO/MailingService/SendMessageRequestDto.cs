namespace TradingAlerter.Entity.DTO.MailingService;

public sealed class SendMessageRequestDto
{
    /// <summary>
    /// Receiver can be in different types.
    /// For example: E-Mail address, Telegram Chat-Id etc.
    /// </summary>
    public string Receiver { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}
