using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using TradingAlerter.Entity.Configuration;
using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.MailingService;

namespace TradingAlerter.Infrastructure.MailingService;

public sealed class EmailMailingProvider : IMailingService, IDisposable
{
    private readonly SmtpClient _smtpClient;
    private readonly ILogger _logger;
    private readonly MailConfig _mailConfig;
    private bool _isDisposed = false;

    public EmailMailingProvider(ILogger logger, MailConfig mailConfig)
    {
        _smtpClient = new SmtpClient(mailConfig.SmtpHost) 
        {
            Port = mailConfig.SmtpPort,
            Credentials = new NetworkCredential(mailConfig.Username, mailConfig.Password),
            EnableSsl = true
        };

        _logger = logger;
        _mailConfig = mailConfig;
    }

    private string GetMailingServiceProviderMessage(SendMessageRequestDto request)
    {
        if(_isDisposed)
            throw new ObjectDisposedException(nameof(EmailMailingProvider));

        var emailTemplateMessage = @"
            <h1>{0}</h1>
            <br>
            <h3>{1}</h3>";
        return string.Format(emailTemplateMessage, request.Title, request.Message);
    }

    public async Task<Response<SendMessageResponseDto>> Send(SendMessageRequestDto request)
    {
        if(_isDisposed)
            throw new ObjectDisposedException(nameof(EmailMailingProvider));

        if(request == null)
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.BadRequest);

        try
        {
            var emailMessage = GetMailingServiceProviderMessage(request);
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailConfig.FromAddress),
                Subject = request.Message,
                Body = emailMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(request.Receiver);

            await _smtpClient.SendMailAsync(mailMessage);

            return Response<SendMessageResponseDto>.SuccessResponse(ErrorCode.Approved, 
                new SendMessageResponseDto
                {
                    IsSent = true,
                    ProviderMessage = "OK"
                });
        }
        catch (SmtpFailedRecipientException failedRecipientEx)
        {
            _logger.LogError(failedRecipientEx, "Failed recipient to send e-mail in EmailMailingProvider, see exception message to get more details.");
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.SmtpReceipientError);
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "Failed to send e-mail in EmailMailingProvider, see exception message to get more details.");
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.SmtpError);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email, some unhandled error was detected, see exception message for more details.");
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.InternalError);
        }
    }

    public void Dispose()
    {
        if(_isDisposed) return;

        _smtpClient.Dispose();

        _isDisposed = true;
    }
}
