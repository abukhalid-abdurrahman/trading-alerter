using System.Threading.Tasks;
using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.MailingService;

namespace TradingAlerter.Infrastructure.MailingService;

public interface IMailingService
{
    /// <summary>
    /// Returns formated message from input SendMessageRequestDto, for mailing provider.
    /// </summary>
    private string GetMailingServiceProviderMessage(SendMessageRequestDto request)
    {
        return request.Message;
    }
    public Task<Response<SendMessageResponseDto>> Send(SendMessageRequestDto request);
}
