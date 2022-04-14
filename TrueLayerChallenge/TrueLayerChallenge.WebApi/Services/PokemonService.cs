using System.Text.RegularExpressions;
using Newtonsoft.Json;
using TrueLayerChallenge.WebApi.Dtos;
using TrueLayerChallenge.WebApi.Extensions;
using TrueLayerChallenge.WebApi.Resources;
using TrueLayerChallenge.WebApi.Schemas.PokeApi;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

/// <inheritdoc />
internal class PokemonService : IPokemonService
{
    private readonly ILogger<PokemonService> _logger;
    private readonly IFunTranslationsService _funTranslationsService;
    private readonly HttpClient _client;
    private bool _disposed;

    private const string Endpoint_PokemonSpecies = "pokemon-species/";

    /// <summary>
    /// Creates a new <see cref="PokemonService"/>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> providing logger functionality within the <see cref="PokemonService"/>.</param>
    /// <param name="httpClient"><see cref="HttpClient"/> instance providing HTTP functionality.</param>
    /// <param name="funTranslationsService"><see cref="IFunTranslationsService"/> for performing fun translations operations.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logger"/> is null.
    /// -or-
    /// <paramref name="httpClient"/> is null.
    /// -or-
    /// <paramref name="funTranslationsService"/> is null.
    /// </exception>
    public PokemonService(ILogger<PokemonService> logger, HttpClient httpClient, IFunTranslationsService funTranslationsService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _funTranslationsService = funTranslationsService ?? throw new ArgumentNullException(nameof(funTranslationsService));
    }

    /// <inheritdoc />
    public async Task<ShakespeareanPokemonDescriptionDto?> GetShakespeareanDescriptionAsync(string pokemonName)
    {
        if (_disposed) throw new ObjectDisposedException(ToString());

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
        if (!_disposed)
        {
            _funTranslationsService?.Dispose();
            _client?.Dispose();
        }

        _disposed = true;
    }

    private async Task<string?> GetPokemonDescriptionAsync(string pokemonName)
    {

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, $"{Endpoint_PokemonSpecies}{pokemonName}");
            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                // future improvement could do something more elaborate here but just for illustration will log failure and return null
                _logger.Log(LogLevel.Error, LogMessages.PokeApi_ErrorResponse, response.StatusCode);
                return null;
            }

            var speciesInfo = await response.DeseriliseJsonContentAsync<PokemonSpecies>();

            // future improvement update could be to allow user to choose different versions for test just use first for illustration
            return Regex.Replace(speciesInfo.flavor_text_entries[0].flavor_text, @"\r\n?|\n|\f", " ");
        }
        catch (HttpRequestException)
        {
            // future improvement implement Polly for retry pipeline here
            _logger.Log(LogLevel.Error, LogMessages.PokeApi_HttpRequestFailed);
            return null;
        }
        catch (JsonSerializationException e)
        {
            _logger.Log(LogLevel.Error, LogMessages.PokeApi_JsonDeserialisationFailed, pokemonName, e.Message);
            return null;
        }
    }

    private async Task<string?> ConvertToShakespeareanAsync(string content)
    {
        return await _funTranslationsService.ConvertToShakespeareanAsync(content);
    }
}