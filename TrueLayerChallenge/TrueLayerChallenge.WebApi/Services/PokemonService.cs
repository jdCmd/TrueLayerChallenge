using Microsoft.Extensions.Options;
using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Dtos;
using TrueLayerChallenge.WebApi.Extensions;
using TrueLayerChallenge.WebApi.Schemas.PokeApi;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

/// <inheritdoc />
internal class PokemonService : IPokemonService
{
    private readonly ILogger<PokemonService> _logger;
    private readonly IFunTranslationsService _funTranslationsService;
    private readonly HttpClient _client;

    private const string Endpoint_PokemonSpecies = "pokemon-species/";

    /// <summary>
    /// Creates a new <see cref="PokemonService"/>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> providing logger functionality within the <see cref="PokemonService"/>.</param>
    /// <param name="config"><see cref="IOptions{TOptions}"/> containing <see cref="PokeApiConfig"/>.</param>
    /// <param name="funTranslationsService"><see cref="IFunTranslationsService"/> for performing fun translations operations.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logger"/> is null.
    /// -or-
    /// <paramref name="funTranslationsService"/> is null.
    /// </exception>
    public PokemonService(ILogger<PokemonService> logger, IOptions<PokeApiConfig> config, IFunTranslationsService funTranslationsService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        if (config == null) throw new ArgumentNullException(nameof(config));
        if (config.Value == null) throw new ArgumentNullException(nameof(config.Value));

        _funTranslationsService = funTranslationsService ?? throw new ArgumentNullException(nameof(funTranslationsService));

        _client = new HttpClient
        {
            BaseAddress = new Uri(config.Value.Url),
            Timeout = new TimeSpan(0, 0, 0, config.Value.ConnectionTimeoutMilliseconds)
        };
    }

    /// <inheritdoc />
    public async Task<ShakespeareanPokemonDescriptionDto?> GetShakespeareanDescriptionAsync(string pokemonName)
    {
        var description = await GetPokemonDescriptionAsync(pokemonName);

        if (description == null) return null;

        return new ShakespeareanPokemonDescriptionDto
        {
            Name = pokemonName,
            // having issues locally communicating with Fun Translator as is blocked by local AV.
            // Hence return actual description if not retrieved so there is something to show!
            Description = await ConvertToShakespeareanAsync(description) ?? description
        };
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _funTranslationsService?.Dispose();
        _client?.Dispose();
    }

    private async Task<string?> GetPokemonDescriptionAsync(string pokemonName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{Endpoint_PokemonSpecies}{pokemonName}");
        using var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            // could do something more elaborate here but just for illustration will log failure and return null
            return null;
        }

        var speciesInfo = await response.DeseriliseJsonContentAsync<PokemonSpecies>();

        // future improvement update could be to allow user to choose different versions for test just use first for illustration

        return speciesInfo.flavor_text_entries[0].flavor_text;
    }

    private async Task<string?> ConvertToShakespeareanAsync(string content)
    {
        return await _funTranslationsService.ConvertToShakespeareanAsync(content);
    }
}