#pragma warning disable CS8618

namespace TrueLayerChallenge.WebApi.Schemas.PokeApi;

public class Flavor_Text_Entries
{
    public string flavor_text { get; set; }
    public Language language { get; set; }
    public Version version { get; set; }
}