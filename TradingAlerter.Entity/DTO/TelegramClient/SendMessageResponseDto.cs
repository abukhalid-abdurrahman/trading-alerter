using Newtonsoft.Json;

namespace TradingAlerter.Entity.DTO.TelegramClient;

public sealed class SendMessageResponseDto
{
    [JsonProperty("message_id")]
    public long MessageId { get; set; }
}