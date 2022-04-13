namespace TrueLayerChallenge.WebApi.Schemas.PokeApi;

public class PokemonSpecies
{
    public int base_happiness { get; set; }
    public int capture_rate { get; set; }
    public Color color { get; set; }
    public Egg_Groups[] egg_groups { get; set; }
    public Evolution_Chain evolution_chain { get; set; }
    public object evolves_from_species { get; set; }
    public Flavor_Text_Entries[] flavor_text_entries { get; set; }
    public object[] form_descriptions { get; set; }
    public bool forms_switchable { get; set; }
    public int gender_rate { get; set; }
    public Genera[] genera { get; set; }
    public Generation generation { get; set; }
    public Growth_Rate growth_rate { get; set; }
    public Habitat habitat { get; set; }
    public bool has_gender_differences { get; set; }
    public int hatch_counter { get; set; }
    public int id { get; set; }
    public bool is_baby { get; set; }
    public bool is_legendary { get; set; }
    public bool is_mythical { get; set; }
    public string name { get; set; }
    public Name[] names { get; set; }
    public int order { get; set; }
    public Pal_Park_Encounters[] pal_park_encounters { get; set; }
    public Pokedex_Numbers[] pokedex_numbers { get; set; }
    public Shape shape { get; set; }
    public Variety[] varieties { get; set; }
}