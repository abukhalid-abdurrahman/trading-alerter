using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.MailingService;

namespace TradingAlerter.Infrastructure.MailingService;

public sealed class EmailMailingProvider : IMailingService
{
    public EmailMailingProvider()
    {
    }

    private string GetMailingServiceProviderMessage(SendMessageRequestDto request)
    {
        return "";
    }

    public Task<Response<SendMessageResponseDto>> Send(SendMessageRequestDto request)
    {
        throw new NotImplementedException();
    }
}
