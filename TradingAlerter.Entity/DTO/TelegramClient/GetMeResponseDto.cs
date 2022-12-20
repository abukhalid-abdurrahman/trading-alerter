using Newtonsoft.Json;

namespace TradingAlerter.Entity.DTO.TelegramClient;

public sealed class GetMeResponseDto
{
    [JsonProperty("id")]
    public long UserId { get; set; }

    [JsonProperty("is_bot")]
    public bool IsBot { get; set; }

    [JsonProperty("first_name")]
    public string FirstName { get; set; }

    [JsonProperty("last_name")]
    public string LastName { get; set; }

    [JsonProperty("username")]
    public string UserName { get; set; }
}
