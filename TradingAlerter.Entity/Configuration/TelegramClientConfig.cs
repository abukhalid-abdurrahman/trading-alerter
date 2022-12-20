namespace TradingAlerter.Entity.Configuration;

public sealed class TelegramClientConfig
{
    public long ChatId { get; set; }
    public string BotToken { get; set; }
    public string BotApiHost { get; set; }
    public string SendMessageEndpoint { get; set; }
    public string GetMeEndpoint { get; set; }
}
