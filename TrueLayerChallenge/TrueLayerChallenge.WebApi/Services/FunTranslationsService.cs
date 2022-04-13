using Microsoft.Extensions.Options;
using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

internal class FunTranslationsService : IFunTranslationsService
{
    private readonly ILogger<FunTranslationsService> _logger;
    private readonly HttpClient _client;

    private const string ShakespeareEndpoint = "shakespeare/";

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
        using var request = new HttpRequestMessage(HttpMethod.Get, ShakespeareEndpoint);
        using var response = await _client.SendAsync(request);
        
        // todo handle response success etc

        return await response.Content.ReadAsStringAsync();
    }

    // could add other translation options such as pig latin etc...

    public void Dispose()
    {
        _client?.Dispose();
    }
}