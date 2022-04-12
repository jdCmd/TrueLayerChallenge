namespace TrueLayerChallenge.WebApi.Services.Interfaces;

/// <summary>
/// Service providing pokemon related functionality.
/// </summary>
internal interface IPokemonService
{
    /// <summary>
    /// Asynchronously gets the shakespearean description for the pokemon with the given <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the pokemon for which to get the shakespearean description for.</param>
    /// <returns><see cref="Task{T}"/> with result containing the shakespearean description of the given pokemon.</returns>
    Task<string> GetShakespeareanDescriptionAsync(string name);
}