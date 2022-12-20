using Newtonsoft.Json;

namespace TradingAlerter.Entity.DTO.TelegramClient;

public sealed class SendMessageRequestDto
{
    [JsonProperty("chat_id")]
    public string ChatId { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }
}
