using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.TelegramClient;

namespace TradingAlerter.Infrastructure.MessangerClient.TelegramClient;

public interface ITelegramClient
{
    /// <summary>
    /// Calls sendMessage method from Telegram Bot API, 
    /// to send message.
    /// </summary>
    public Task<Response<SendMessageResponseDto>> SendMessage(SendMessageRequestDto requestDto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Calls getMe method from Telegram Bot API, 
    /// to validate token and get bot information.
    /// </summary>
    public Task<Response<GetMeResponseDto>> GetMe(CancellationToken cancellationToken = default);
}