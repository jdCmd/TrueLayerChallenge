using TrueLayerChallenge.WebApi.Dtos;
using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

/// <inheritdoc />
internal class PokemonService : IPokemonService
{
    private readonly ILogger<PokemonService> _logger;
    private readonly IShakespeareanConverterService _shakespeareanConverter;

    /// <summary>
    /// Creates a new <see cref="PokemonService"/>.
    /// </summary>
    /// <param name="logger"><see cref="ILogger"/> providing logger functionality within the <see cref="PokemonService"/>.</param>
    /// <param name="shakespeareanConverter"><see cref="IShakespeareanConverterService"/> for converting pokemon descriptions to Shakespearean.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="logger"/> is null.
    /// -or-
    /// <paramref name="shakespeareanConverter"/> is null.
    /// </exception>
    public PokemonService(ILogger<PokemonService> logger, IShakespeareanConverterService shakespeareanConverter)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _shakespeareanConverter = shakespeareanConverter ?? throw new ArgumentNullException(nameof(shakespeareanConverter));
    }

    /// <inheritdoc />
    public async Task<ShakespeareanPokemonDescriptionDto?> GetShakespeareanDescriptionAsync(string pokemonName)
    {
        var description = await GetPokemonDescriptionAsync(pokemonName);

        // todo return null if not found

        return new ShakespeareanPokemonDescriptionDto
        {
            Name = pokemonName,
            Description = await ConvertToShakespeareanAsync(description)
        };
    }

    private async Task<string> GetPokemonDescriptionAsync(string pokemon)
    {
        return await Task.Run(() => "some description");
    }

    private async Task<string> ConvertToShakespeareanAsync(string content)
    {
        return await _shakespeareanConverter.ConvertToShakespeareanAsync(content);
    }
}