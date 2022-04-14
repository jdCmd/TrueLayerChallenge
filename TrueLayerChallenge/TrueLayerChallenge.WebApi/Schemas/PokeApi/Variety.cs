#pragma warning disable CS8618

namespace TrueLayerChallenge.WebApi.Schemas.PokeApi;

public class Variety
{
    public bool is_default { get; set; }
    public Pokemon pokemon { get; set; }
}