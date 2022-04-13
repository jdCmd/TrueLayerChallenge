namespace TrueLayerChallenge.WebApi.Dtos
{
    /// <summary>
    /// Data transfer object containing the Shakespearean description of a given pokemon.
    /// </summary>
    public class ShakespeareanPokemonDescriptionDto
    {
        /// <summary>
        /// Name of the pokemon for which the <see cref="ShakespeareanPokemonDescriptionDto"/> corresponds to.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The Shakespearean description of the pokemon.
        /// </summary>
        public string Description { get; set; } = null!;
    }
}
