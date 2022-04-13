using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Enums;
using TrueLayerChallenge.WebApi.Schemas.FunTranslations;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

internal class FunTranslationsService : IFunTranslationsService
{
    private readonly ILogger<FunTranslationsService> _logger;
    private readonly HttpClient _client;

    private const string ShakespeareEndpoint = "shakespeare.json";

    public FunTranslationsService(ILogger<FunTranslationsService> logger, IOptions<FunTranslationsConfig> config)
    {
        _logger = logger;

        if(config == null) throw new ArgumentNullException(nameof(config));
        if(config.Value == null) throw new ArgumentNullException(nameof(config.Value));

        _client = new HttpClient
        {
            BaseAddress = new Uri(config.Value.Url),
            Timeout = new TimeSpan(0, 0, 0, config.Value.ConnectionTimeoutMilliseconds)
        };
    }

    public async Task<string> ConvertToShakespeareanAsync(string content)
    {
        var encodedContent = HttpUtility.UrlEncode(content);
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{ShakespeareEndpoint}?text={encodedContent}");
        using var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            // todo handle response error etc
        }

        var translation = DeserialiseToTranslation(await response.Content.ReadAsStringAsync(), FunTranslation.Shakespeare);
        return translation.contents.translated;
    }

    // could add other translation options such as pig latin etc...

    public void Dispose()
    {
        _client?.Dispose();
    }

    private static Translation DeserialiseToTranslation(string content, FunTranslation funTranslation)
    {
        return JsonConvert.DeserializeObject<Translation>(content) ?? throw new JsonSerializationException(
            $"Failed to deserialise response from Fun Translations. Translation: {funTranslation}");
    }
}