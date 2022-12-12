namespace TradingAlerter.Entity.Configuration;

public sealed class MailConfig
{
    public string FromAddress { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string SmtpHost { get; set; }
    public int SmtpPort { get; set; }
}
