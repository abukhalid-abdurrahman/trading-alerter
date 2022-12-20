#nullable disable

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TradingAlerter.Entity.Configuration;
using TradingAlerter.Entity.DTO;
using TradingAlerter.Entity.DTO.TelegramClient;

namespace TradingAlerter.Infrastructure.MessangerClient.TelegramClient;

public sealed class TelegramClient : ITelegramClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly TelegramClientConfig _telegramClientConfig;

    public TelegramClient(
        ILogger logger,
        IHttpClientFactory httpClientFactory,
        TelegramClientConfig telegramClientConfig)
    {
        _httpClient = httpClientFactory.CreateClient("TelegramClient");
        _logger = logger;
        _telegramClientConfig = telegramClientConfig;
    }

    public async Task<Response<GetMeResponseDto>> GetMe(CancellationToken cancellationToken = default)
    {
        var requestUri = _httpClient.BaseAddress + _telegramClientConfig.GetMeEndpoint;
        var errorMessageTemplate 
            = "GetMe method execution failed, request failed. Address: {0}, error: {1}, message: {2}!";
        try
        {
            var httpResponseMessage = await _httpClient.GetAsync(requestUri, cancellationToken);
            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError(string.Format(errorMessageTemplate, requestUri, httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase));
                return Response<GetMeResponseDto>.FailedResponse(ErrorCode.UpstreamServiceUnavailable);
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            var getMeResponse = JsonConvert.DeserializeObject<GetMeResponseDto>(responseContent);
            
            return Response<GetMeResponseDto>.SuccessResponse(ErrorCode.Approved, getMeResponse);
        }
        catch (TimeoutException tex)
        {
            _logger.LogError(tex, string.Format(errorMessageTemplate, requestUri, tex.Message, tex.Source));
            return Response<GetMeResponseDto>.FailedResponse(ErrorCode.Timeout);
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format(errorMessageTemplate, requestUri, ex.Message, ex.Source));
            return Response<GetMeResponseDto>.FailedResponse(ErrorCode.InternalError);
        }
    }

    public async Task<Response<SendMessageResponseDto>> SendMessage(SendMessageRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var requestUri = _httpClient.BaseAddress + _telegramClientConfig.SendMessageEndpoint;
        var errorMessageTemplate 
            = "SendMessage method execution failed, request failed. Address: {0}, error: {1}, message: {2}, request: {3}!";
        try
        {
            var httpRequestContent = JsonConvert.SerializeObject(requestDto);
            var httpRequestMessage = new HttpRequestMessage() 
            {
                Method = HttpMethod.Post,
                Content = new StringContent(httpRequestContent),
                RequestUri = new Uri(requestUri)
            };
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);
            if(!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogError(string.Format(errorMessageTemplate, requestUri, 
                    httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase, httpRequestContent));
                return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.UpstreamServiceUnavailable);
            }

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            _logger.LogInformation($@"SendMessage method execution complete. 
                Address: {requestUri}, request: {httpRequestContent}, response: {responseContent}!");

            var sendMessageResponse = JsonConvert.DeserializeObject<SendMessageResponseDto>(responseContent);
            
            return Response<SendMessageResponseDto>.SuccessResponse(ErrorCode.Approved, sendMessageResponse);
        }
        catch (TimeoutException tex)
        {
            _logger.LogError(tex, string.Format(errorMessageTemplate, requestUri, tex.Message, tex.Source));
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.Timeout);
        }
        catch (Exception ex)
        {
            _logger.LogError(string.Format(errorMessageTemplate, requestUri, ex.Message, ex.Source));
            return Response<SendMessageResponseDto>.FailedResponse(ErrorCode.InternalError);
        }
    }
}
