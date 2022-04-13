using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Dtos;
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
            Description = await ConvertToShakespeareanAsync(description)
        };
    }

    private async Task<string?> GetPokemonDescriptionAsync(string pokemonName)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{Endpoint_PokemonSpecies}{pokemonName}");
        using var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {

        }

        var speciesInfo = JsonConvert.DeserializeObject<PokemonSpecies>(await response.Content.ReadAsStringAsync());
        
        // todo future improvement update so can use description for different versions for test just use first
        return speciesInfo.flavor_text_entries[0].flavor_text;
    }

    private async Task<string> ConvertToShakespeareanAsync(string content)
    {
        return await _funTranslationsService.ConvertToShakespeareanAsync(content);
    }

    public void Dispose()
    {
        _funTranslationsService?.Dispose();
        _client?.Dispose();
    }
}