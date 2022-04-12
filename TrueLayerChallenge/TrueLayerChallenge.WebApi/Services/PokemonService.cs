using TrueLayerChallenge.WebApi.Services.Interfaces;

namespace TrueLayerChallenge.WebApi.Services;

/// <inheritdoc />
internal class PokemonService : IPokemonService
{
    private readonly ILogger<PokemonService> _logger;

    public PokemonService(ILogger<PokemonService> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> GetShakespeareanDescriptionAsync(string name)
    {
        var description = await GetPokemonDescriptionAsync(name);
        return await ConvertToShakepeareanAsync(description);
    }

    private async Task<string> GetPokemonDescriptionAsync(string pokemon)
    {
        return "";
    }

    private async Task<string> ConvertToShakepeareanAsync(string content)
    {
        return "";
    }
}