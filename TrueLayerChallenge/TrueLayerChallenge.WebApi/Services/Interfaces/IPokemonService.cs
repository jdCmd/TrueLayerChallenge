using TrueLayerChallenge.WebApi.Dtos;

namespace TrueLayerChallenge.WebApi.Services.Interfaces;

/// <summary>
/// Service providing pokemon related functionality.
/// </summary>
public interface IPokemonService : IDisposable
{
    /// <summary>
    /// Asynchronously gets the shakespearean description for the pokemon with the given <paramref name="pokemonName"/>.
    /// </summary>
    /// <param name="pokemonName">Name of the pokemon for which to get the shakespearean description for.</param>
    /// <returns><see cref="ShakespeareanPokemonDescriptionDto"/> with result containing the shakespearean description of the given pokemon. If pokemon is not found, returns null.</returns>
    /// <exception cref="ObjectDisposedException">The <see cref="IPokemonService"/> instance has been disposed.</exception>
    Task<ShakespeareanPokemonDescriptionDto?> GetShakespeareanDescriptionAsync(string pokemonName);
}