using Microsoft.Extensions.Logging;
using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.MailingService;
using TradingAlerter.Infrastructure.MessangerClient.TelegramClient;

namespace TradingAlerter.Infrastructure.MailingService;

public sealed class TelegramMailingProvider : IMailingService
{
    private readonly ITelegramClient _telegramClient;
    private readonly ILogger _logger;

    public TelegramMailingProvider(
        ITelegramClient telegramClient, 
        ILogger logger)
    {
        _telegramClient = telegramClient;
        _logger = logger;
    }

    private string GetMailingServiceProviderMessage(SendMessageRequestDto request)
    {
        return $@"*bold \*{request.Title}*
            \n{request.Message}";
    }

    public async Task<Response<SendMessageResponseDto>> Send(SendMessageRequestDto request, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = GetMailingServiceProviderMessage(request);

            var getMeResponse = await _telegramClient.GetMe(cancellationToken);
            if(getMeResponse.Code != ErrorCode.Approved)
            {
                _logger.LogError($"Failed to authorize telegram client (bot), message sending failed, error: {getMeResponse.Message}");
                return Response<SendMessageResponseDto>.FailedResponse(getMeResponse.Code, getMeResponse.Message); 
            }

            var sendMessageResponse = await _telegramClient.SendMessage(
                new Entity.DTO.TelegramClient.SendMessageRequestDto 
                {
                    ChatId = request.Receiver,
                    Text = message
                }, cancellationToken);

            if(sendMessageResponse.Code != ErrorCode.Approved)
            {
                _logger.LogError($"Failed to send message, error: {getMeResponse.Message}");
                return Response<SendMessageResponseDto>.FailedResponse(getMeResponse.Code, getMeResponse.Message); 
            }

            return Response<SendMessageResponseDto>.SuccessResponse(ErrorCode.Approved, 
                new SendMessageResponseDto() 
                {
                    IsSent = true,
                    ProviderMessage = sendMessageResponse.Payload?.MessageId.ToString()
                });
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Failed to send message to telegram, see exception message for more details.");
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.InternalError);
        }
    }
}
