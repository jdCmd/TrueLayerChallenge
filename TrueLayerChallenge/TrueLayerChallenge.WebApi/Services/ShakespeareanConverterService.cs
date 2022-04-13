using Microsoft.Extensions.Options;
using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

internal class ShakespeareanConverterService : IShakespeareanConverterService
{
    private readonly ILogger<ShakespeareanConverterService> _logger;

    public ShakespeareanConverterService(ILogger<ShakespeareanConverterService> logger, IOptions<ShakespeareanTranslatorConfig> config)
    {
        _logger = logger;

        // todo implement with http client etc.
        _ = config.Value ?? throw new ArgumentNullException(nameof(config.Value));
    }

    public Task<string> ConvertToShakespeareanAsync(string content)
    {
        throw new NotImplementedException();
    }
}