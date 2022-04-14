using System.Web;
using Newtonsoft.Json;
using TrueLayerChallenge.WebApi.Extensions;
using TrueLayerChallenge.WebApi.Resources;
using TrueLayerChallenge.WebApi.Schemas.FunTranslations;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

internal class FunTranslationsService : IFunTranslationsService
{
    private readonly ILogger<FunTranslationsService> _logger;
    private readonly HttpClient _client;
    private bool _disposed;

    private const string ShakespeareEndpoint = "shakespeare.json";

    public FunTranslationsService(ILogger<FunTranslationsService> logger, HttpClient httpClient)
    {
        _logger = logger;
        _client = httpClient;
    }

    public async Task<string?> ConvertToShakespeareanAsync(string content)
    {
        if (_disposed) throw new ObjectDisposedException(ToString());

        try
        {
            var encodedContent = HttpUtility.UrlEncode(content);
            using var request = new HttpRequestMessage(HttpMethod.Post, $"{ShakespeareEndpoint}?text={encodedContent}");
            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // future improvement could do something more elaborate here but just for illustration will log failure and return null
                _logger.Log(LogLevel.Warning, LogMessages.FunTranslation_ErrorResponse, response.StatusCode);
                return null;
            }

            var translation = await response.DeseriliseJsonContentAsync<Translation>();
            return translation.contents.translated;
        }
        catch (HttpRequestException)
        {
            // future improvement implement Polly for retry pipeline here
            _logger.Log(LogLevel.Error, LogMessages.FunTranslation_HttpRequestFailed);
            return null;
        }
        catch (JsonSerializationException e)
        {
            _logger.Log(LogLevel.Error, LogMessages.PokeApi_JsonDeserialisationFailed, content, e.Message);
            return null;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if(!_disposed) _client?.Dispose();

        _disposed = true;
    }
}