using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.MailingService;

namespace TradingAlerter.Infrastructure.MailingService;

public sealed class TelegramMailingProvider : IMailingService
{
    public TelegramMailingProvider()
    {
    }

    private string GetMailingServiceProviderMessage(SendMessageRequestDto request)
    {
        return request.Message;
    }

    public Task<Response<SendMessageResponseDto>> Send(SendMessageRequestDto request)
    {
        throw new NotImplementedException();
    }
}
