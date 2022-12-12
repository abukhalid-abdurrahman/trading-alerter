namespace TradingAlerter.Entity.DTO.MailingService;

public sealed class SendMessageResponseDto
{
    public bool IsSent { get; set; }
    public string ProviderMessage { get; set; }
}
